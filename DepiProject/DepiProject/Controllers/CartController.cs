using DataLayer.Repository.IRepository;
using DepiProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
            {                // Get the product to ensure it exists and to get its price
                var product = _unitofOfWork.Product.Get(p => p.ProductId == productId);
                if (product == null)
                {
                    // Product doesn't exist, return an error
                    TempData["Error"] = "Product not found.";
                    return RedirectToAction("Index", "Home");
                }

                _unitofOfWork.ShoppingCart.Add(new ShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    Count = quantity,
                    Price = product.Price
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

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofOfWork.User.GetUser(userId);


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
        [HttpGet]
        public IActionResult Checkout()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitofOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product,Product.ProductImages"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofOfWork.User.GetUser(userId);

            if (ShoppingCartVM.OrderHeader.ApplicationUser != null)
            {
                ShoppingCartVM.OrderHeader.FirstName = ShoppingCartVM.OrderHeader.ApplicationUser.FirstName;
                ShoppingCartVM.OrderHeader.LastName = ShoppingCartVM.OrderHeader.ApplicationUser.LastName;
                ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
                ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
                ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
                ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
                ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
                ShoppingCartVM.OrderHeader.Country = "USA"; // Default value, adjust as needed
            }

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
            var domain = "http://localhost:5086/";
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
            OrderHeader orderHeader = _unitofOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

            if (orderHeader == null)
            {
                return NotFound();
            }

            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new Stripe.Checkout.SessionService();
                Stripe.Checkout.Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitofOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitofOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitofOfWork.Save();
                }

                List<ShoppingCart> shoppingCarts = _unitofOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
                _unitofOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                _unitofOfWork.Save();
            }

            // Get order details for the confirmation page
            var orderDetails = _unitofOfWork.OrderDetails.GetAll(
                od => od.OrderHeaderId == id,
                includeProperties: "Product,Product.ProductImages"
            ).ToList();

            var orderConfirmation = new OrderConfirmationViewModel
            {
                OrderId = orderHeader.Id,
                OrderDate = orderHeader.OrderDate,
                ShippingAddress = $"{orderHeader.FirstName} {orderHeader.LastName}, {orderHeader.StreetAddress}, {orderHeader.City}, {orderHeader.State} {orderHeader.PostalCode}, {orderHeader.Country}",
                PaymentMethod = "Credit Card", // Adjust as needed based on your payment methods
                CustomerName = $"{orderHeader.FirstName} {orderHeader.LastName}",
                TrackingNumber = orderHeader.TrackingNumber ?? "Not available yet",
                OrderStatus = orderHeader.OrderStatus,
                TotalPrice = (decimal)orderHeader.OrderTotal,
                Items = new List<OrderItemViewModel>()
            };

            foreach (var detail in orderDetails)
            {
                orderConfirmation.Items.Add(new OrderItemViewModel
                {
                    Id = detail.ProductId,
                    Name = detail.Product.Name,
                    Image = detail.Product.ProductImages.FirstOrDefault()?.Path ?? "/images/no-image.jpg",
                    Count = detail.Count,
                    Price = (decimal)detail.Price
                });
            }

            return View(orderConfirmation);
        }
        [HttpPost]
        public IActionResult PlaceOrder(OrderHeader orderHeader)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var shoppingCarts = _unitofOfWork.ShoppingCart.GetAll(
                a => a.ApplicationUserId == userId,
                includeProperties: "Product,Product.ProductImages"
            ).ToList();

            if (shoppingCarts.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            // Create and populate the order header
            OrderHeader newOrder = new()
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now,
                FirstName = orderHeader.FirstName,
                LastName = orderHeader.LastName,
                PhoneNumber = orderHeader.PhoneNumber,
                StreetAddress = orderHeader.StreetAddress,
                City = orderHeader.City,
                State = orderHeader.State,
                PostalCode = orderHeader.PostalCode,
                Country = orderHeader.Country,
                OrderStatus = SD.StatusPending,
                PaymentStatus = SD.PaymentStatusPending,
                TrackingNumber = Guid.NewGuid().ToString().Substring(0, 10).ToUpper(),
                OrderTotal = 0
            };

            // Calculate order total
            foreach (var cart in shoppingCarts)
            {
                newOrder.OrderTotal += (double)(cart.Product.Price * cart.Count);
            }

            // Add order header to database
            _unitofOfWork.OrderHeader.Add(newOrder);
            _unitofOfWork.Save();

            // Create order details for each cart item
            foreach (var cart in shoppingCarts)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = newOrder.Id,
                    Price = (double)cart.Product.Price,
                    Count = cart.Count
                };
                _unitofOfWork.OrderDetails.Add(orderDetails);
            }
            _unitofOfWork.Save();

            // Process payment via Stripe
            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Cart/OrderConfirmation?id={newOrder.Id}",
                CancelUrl = domain + "Cart/Checkout",
            };

            foreach (var item in shoppingCarts)
            {
                var description = item.Product.Description;
                if (description != null && description.Length > 100)
                {
                    description = description.Substring(0, 100);
                }

                var sessionItem = new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // Stripe uses cents
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                            Description = description
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionItem);
            }

            // Create Stripe checkout session
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            // Update order with Stripe session information
            _unitofOfWork.OrderHeader.UpdateStripePaymentID(newOrder.Id, session.Id, session.PaymentIntentId);
            _unitofOfWork.Save();

            // Redirect to Stripe checkout
            Response.Headers["Location"] = session.Url;
            return new StatusCodeResult(303);
        }
    }
}
