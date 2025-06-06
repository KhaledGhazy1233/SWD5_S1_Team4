﻿using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using DepiProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DepiProject.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitofOfWork;
        private readonly IConfiguration _configuration;
        public ShoppingCartVM ShoppingCartVM { get; set; } = new();

        public CartController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitofOfWork = unitOfWork;
            _configuration = configuration;

            // Ensure Stripe API key is set
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Get<string>();
        }
        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                // For AJAX requests, return a JSON response indicating authentication is required
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, requiresLogin = true, message = "Please log in to add items to your cart." });
                }
                return RedirectToAction("Login", "Account");
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            var cartItem = await Task.FromResult(_unitofOfWork.ShoppingCart.Get(c => c.ApplicationUserId == userId && c.ProductId == productId));

            if (cartItem != null)
            {
                cartItem.Count += quantity;
                // Change from await to use Task.Run since Update doesn't return a Task
                _unitofOfWork.ShoppingCart.Update(cartItem);
                await Task.Run(() => _unitofOfWork.Save());
            }
            else
            {
                // Get the product to ensure it exists and to get its price
                var product = await Task.FromResult(_unitofOfWork.Product.Get(p => p.ProductId == productId));
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

            // Use Task.Run for synchronous operations to make them async
            await Task.Run(() => _unitofOfWork.Save());

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // If AJAX request, return JSON with cart count
                var cartCount = _unitofOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId)
                    .Sum(c => c.Count);

                return Json(new { success = true, count = cartCount });
            }

            // If not AJAX, redirect back to the referring page or home page
            if (!string.IsNullOrEmpty(Request.Headers["Referer"]))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Plus(int cartId)
        {
            var cartfromdb = await Task.FromResult(_unitofOfWork.ShoppingCart.Get(s => s.Id == cartId));
            if (cartfromdb == null)
            {
                return RedirectToAction(nameof(Index));
            }
            cartfromdb.Count += 1;
            _unitofOfWork.ShoppingCart.Update(cartfromdb);
            await Task.Run(() => _unitofOfWork.Save());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Minus(int cartId)
        {
            var cartfromdb = await Task.FromResult(_unitofOfWork.ShoppingCart.Get(s => s.Id == cartId));
            if (cartfromdb == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (cartfromdb.Count <= 1)
            {
                _unitofOfWork.ShoppingCart.Remove(cartfromdb);
            }
            else
            {
                cartfromdb.Count -= 1;
                _unitofOfWork.ShoppingCart.Update(cartfromdb);
            }
            await Task.Run(() => _unitofOfWork.Save());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Remove(int cartId)
        {
            var cartfromdb = await Task.FromResult(_unitofOfWork.ShoppingCart.Get(s => s.Id == cartId));
            if (cartfromdb == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _unitofOfWork.ShoppingCart.Remove(cartfromdb);
            await Task.Run(() => _unitofOfWork.Save());
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Reset OrderTotal to 0
            ShoppingCartVM.OrderTotal = 0;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        // Get shopping cart items with product and product images
                        ShoppingCartVM.ShoppingCartList = await Task.FromResult(_unitofOfWork.ShoppingCart
                            .GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product,Product.ProductImages"));

                        // Initialize order header if needed
                        if (ShoppingCartVM.OrderHeader == null)
                        {
                            ShoppingCartVM.OrderHeader = new OrderHeader();
                        }

                        // Set application user
                        ShoppingCartVM.OrderHeader.ApplicationUser = _unitofOfWork.User.GetUser(userId);

                        // Calculate total
                        foreach (var cart in ShoppingCartVM.ShoppingCartList)
                        {
                            ShoppingCartVM.OrderTotal += (double)(cart.Price * cart.Count);
                        }
                    }
                }
            }

            return View(ShoppingCartVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<int> GetCartCount()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return 0;
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return 0;
            }

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return 0;
            }

            var cartItems = await Task.FromResult(_unitofOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId));
            return cartItems.Sum(c => c.Count);
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (User.Identity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ShoppingCartVM = new()
            {
                ShoppingCartList = await Task.FromResult(_unitofOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product,Product.ProductImages")),
                OrderHeader = new()
            }; ShoppingCartVM.OrderHeader.ApplicationUser = await Task.FromResult(_unitofOfWork.User.GetUser(userId));

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
        public async Task<IActionResult> SummaryPost()
        {
            if (User.Identity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ShoppingCartVM.ShoppingCartList = await Task.FromResult(_unitofOfWork.ShoppingCart.GetAll(a => a.ApplicationUserId == userId, includeProperties: "Product")); ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (double)(cart.Product.Price * cart.Count);
            }

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

            _unitofOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            await Task.Run(() => _unitofOfWork.Save());

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
            await Task.Run(() => _unitofOfWork.Save());            // Stripe Checkout Session
            var domain = "http://localhost:5086/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
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
            var session = await service.CreateAsync(options);

            _unitofOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            await Task.Run(() => _unitofOfWork.Save());

            Response.Headers["Location"] = session.Url;
            return new StatusCodeResult(303); // Redirect to Stripe Checkout
        }
        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            try
            {
                OrderHeader orderHeader = await Task.FromResult(_unitofOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser"));

                if (orderHeader == null)
                {
                    return NotFound();
                }

                if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
                {
                    var service = new Stripe.Checkout.SessionService();
                    Stripe.Checkout.Session session = await service.GetAsync(orderHeader.SessionId);

                    if (session.PaymentStatus.ToLower() == "paid")
                    {
                        _unitofOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                        _unitofOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                        await Task.Run(() => _unitofOfWork.Save());
                    }

                    List<ShoppingCart> shoppingCarts = await Task.FromResult(_unitofOfWork.ShoppingCart
                        .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList());
                    _unitofOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                    await Task.Run(() => _unitofOfWork.Save());
                }                // Get order details for the confirmation page
                var orderDetails = await Task.FromResult(_unitofOfWork.OrderDetails.GetAll(
                    od => od.OrderHeaderId == id,
                    includeProperties: "Product,Product.ProductImages"
                ).ToList());

                var orderConfirmation = new OrderConfirmationViewModel
                {
                    OrderId = orderHeader.Id,
                    OrderDate = orderHeader.OrderDate,
                    ShippingAddress = $"{orderHeader.FirstName} {orderHeader.LastName}, {orderHeader.StreetAddress}, {orderHeader.City}, {orderHeader.State} {orderHeader.PostalCode}, {orderHeader.Country}",
                    PaymentMethod = "Stripe", // Using Stripe for payment processing
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
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in OrderConfirmation: {ex.Message}");

                // For development, you might want to see the full error
                return Content($"Error processing order: {ex.Message}. {ex.StackTrace}");

                // For production, redirect to a friendly error page
                // return RedirectToAction("Error", "Home", new { message = "There was an error processing your order." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderHeader orderHeader)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var shoppingCarts = await Task.FromResult(_unitofOfWork.ShoppingCart.GetAll(
                a => a.ApplicationUserId == userId,
                includeProperties: "Product,Product.ProductImages"
            ).ToList());

            if (shoppingCarts.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }            // Create and populate the order header
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
                Carrier = "Pending", // Setting a default value for Carrier to prevent NULL constraint violation
                OrderTotal = 0
            };

            // Calculate order total
            foreach (var cart in shoppingCarts)
            {
                newOrder.OrderTotal += (double)(cart.Product.Price * cart.Count);
            }

            // Add order header to database
            _unitofOfWork.OrderHeader.Add(newOrder);
            await Task.Run(() => _unitofOfWork.Save());            // Create order details for each cart item
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
            await Task.Run(() => _unitofOfWork.Save());

            Order order = new()
            {
                NumberOfProduct = shoppingCarts.Count,
                TotalPrice = (decimal)newOrder.OrderTotal,
                Discount = 0,
                Fax = 0,
                FinalPrice = (decimal)newOrder.OrderTotal,
                AddressDelivered = orderHeader.StreetAddress,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                PhoneNumber = orderHeader.PhoneNumber,
                StreetAddress = orderHeader.StreetAddress,
                City = orderHeader.City,
                State = orderHeader.State,
                PostalCode = orderHeader.PostalCode,
                Name = $"{orderHeader.FirstName} {orderHeader.LastName}",
                ApplicationUserId = userId
            };

            // Add order to the database
            _unitofOfWork.Orders.Add(order);
            await Task.Run(() => _unitofOfWork.Save());            // Process payment via Stripe
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
                // Skip items with null Product to avoid NullReferenceException
                if (item.Product == null)
                {
                    continue;
                }

                // Safely handle Description
                string description = string.Empty;
                if (item.Product.Description != null)
                {
                    description = item.Product.Description;
                    if (description.Length > 100)
                    {
                        description = description.Substring(0, 100);
                    }
                }

                var sessionItem = new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // Stripe uses cents
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name ?? "Product",
                            Description = string.IsNullOrEmpty(description) ? "No description available" : description
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionItem);
            }

            // Create Stripe checkout session
            var service = new Stripe.Checkout.SessionService();

            // Check if we have valid line items before creating a session
            if (options.LineItems.Count == 0)
            {
                // No valid items to purchase
                TempData["Error"] = "No valid products found in your cart.";
                return RedirectToAction("Index", "Cart");
            }

            Stripe.Checkout.Session session = await service.CreateAsync(options);

            // Update order with Stripe session information
            _unitofOfWork.OrderHeader.UpdateStripePaymentID(newOrder.Id, session.Id, session.PaymentIntentId);
            await Task.Run(() => _unitofOfWork.Save());

            // Redirect to Stripe checkout
            Response.Headers["Location"] = session.Url;
            return new StatusCodeResult(303);
        }
    }
}