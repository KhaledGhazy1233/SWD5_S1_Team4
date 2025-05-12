using DataLayer.Repository.IRepository;
//using DepiProject.Models;
using DepiProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
namespace DepiProject.Controllers
{
    public class CartController : Controller
    {
        private IUnitOfWork _unitofOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitofOfWork = unitOfWork;
        }
        [Authorize]
        public IActionResult AddToCart(int productId, int quantity)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItem = _unitofOfWork.ShoppingCart.Get(c => c.ApplicationUserId == userId && c.ProductId == productId);


            if (cartItem != null)
            {
                cartItem.Count += quantity;

                _unitofOfWork.ShoppingCart.Update(cartItem);
            }
            else
            {
                _unitofOfWork.ShoppingCart.Add(new ShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    Count = quantity,
                });
            }

            _unitofOfWork.Save();
            return RedirectToAction("Index", "Home", new { id = productId });
        }
        public IActionResult Plus(int cartId)
        {
            var cartfromdb = _unitofOfWork.ShoppingCart.Get(s => s.Id == cartId);
            cartfromdb.Count += 1;
            _unitofOfWork.ShoppingCart.Update(cartfromdb);
            _unitofOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cartfromdb = _unitofOfWork.ShoppingCart.Get(s => s.Id == cartId);
            if (cartfromdb.Count <= 1)
            {
                _unitofOfWork.ShoppingCart.Remove(cartfromdb);
            }
            else
            {
                cartfromdb.Count -= 1;
                _unitofOfWork.ShoppingCart.Update(cartfromdb);
            }
            _unitofOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartfromdb = _unitofOfWork.ShoppingCart.Get(s => s.Id == cartId);
            _unitofOfWork.ShoppingCart.Remove(cartfromdb);
            _unitofOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitofOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofOfWork.ApplicationUser.Get(u => u.Id == userId);


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (double)(cart.Product.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        //AddInIConCartINNavBar(Header)
        [AllowAnonymous]
        public int GetCartCount()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return 0;
            }
            else
                return _unitofOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId).Sum(c => c.Count);
        }
        public IActionResult SummaryOrder()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitofOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofOfWork.ApplicationUser.Get(u => u.Id == userId);
            ShoppingCartVM.OrderHeader.FirstName = ShoppingCartVM.OrderHeader.ApplicationUser.FirstName;
            ShoppingCartVM.OrderHeader.LastName = ShoppingCartVM.OrderHeader.ApplicationUser.LastName;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.Country = ShoppingCartVM.OrderHeader.ApplicationUser.Country;
            //ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;



            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (double)(cart.Product.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }
        [HttpPost]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ShoppingCartVM.ShoppingCartList = _unitofOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (double)(cart.Product.Price * cart.Count);
            }

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

            _unitofOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitofOfWork.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = (double)cart.Product.Price,
                    Count = cart.Count
                };
                _unitofOfWork.OrderDetails.Add(orderDetails);
            }
            _unitofOfWork.Save();

            // Stripe Checkout Session
            var domain = "http://localhost:5086/"; // غيره حسب عنوان موقعك
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + "Customer/Cart/Index",
            };

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                var sessionItem = new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // Stripe uses cents
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionItem);
            }

            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);

            _unitofOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitofOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303); // Redirect to Stripe Checkout


        }
        public IActionResult OrderConfirmation(int id)
        {
            // استرجاع الـ OrderHeader
            OrderHeader orderHeader = _unitofOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

            if (orderHeader == null)
            {
                return NotFound();
            }

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {

                var service = new Stripe.Checkout.SessionService();
                Stripe.Checkout.Session session = service.Get(orderHeader.SessionId);
                // استرجاع تفاصيل الجلسة من Stripe

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    // إذا كانت حالة الدفع "مدفوعة"
                    _unitofOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);  // تحديث بيانات الدفع
                    _unitofOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);  // تحديث حالة الطلب
                    _unitofOfWork.Save();  // حفظ التغييرات
                }

                // إزالة جميع العناصر من سلة التسوق للمستخدم المرتبط بالطلب
                List<ShoppingCart> shoppingCarts = _unitofOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
                _unitofOfWork.ShoppingCart.RemoveRange(shoppingCarts);  // إزالة العناصر
                _unitofOfWork.Save();  // حفظ التغييرات
            }

            // إرجاع عرض تأكيد الطلب
            return View(id);  // أو يمكنك إرجاع شيء آخر مثل إعادة توجيه إلى صفحة معينة
        }

    }
}
