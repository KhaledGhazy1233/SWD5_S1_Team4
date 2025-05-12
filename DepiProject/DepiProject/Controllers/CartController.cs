using DepiProject.Models;
using DepiProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var shoppingcartvm = new ShoppingCartVM
            {
                ShoppingCart = new List<ShoppingCart>
    {
        new ShoppingCart
        {
            Id = 1,
            Name = "Product 1",
            Description = "Description of Product 1",
            Price = 10.0,
            Count = 2,
            Image = "image1.jpg"
        },
        new ShoppingCart
        {
            Id = 2,
            Name = "Product 2",
            Description = "Description of Product 2",
            Price = 20.0,
            Count = 1,
            Image = "image2.jpg"
        }
    },
                TotalPrice = 10.0 * 2 + 20.0 * 1 // = 40.0
            };

            return View(shoppingcartvm);
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            // Prepare the shopping cart data for the checkout page
            var shoppingcartvm = new ShoppingCartVM
            {
                ShoppingCart = new List<ShoppingCart>
                {
                    new ShoppingCart
                    {
                        Id = 1,
                        Name = "High-Performance Laptop",
                        Description = "16GB RAM, 512GB SSD, Intel Core i7",
                        Price = 999.99,
                        Count = 1,
                        Image = "https://tse4.mm.bing.net/th/id/OIP.okPHK-lOk_E5nzOZsGx2dwHaFI?cb=iwc2&rs=1&pid=ImgDetMain"
                    },
                    new ShoppingCart
                    {
                        Id = 2,
                        Name = "Premium Smartphone",
                        Description = "128GB Storage, 8GB RAM, 108MP Camera",
                        Price = 799.99,
                        Count = 1,
                        Image = "https://purepng.com/public/uploads/large/purepng.com-mobile-phone-with-touchmobilemobile-phonehandymobile-devicetouchscreenmobile-phone-device-231519333033crymn.png"
                    }
                },
                TotalPrice = 999.99 + 799.99 // = 1799.98
            };

            // Render the checkout page
            return View(shoppingcartvm);
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            // In a real application, this would:
            // 1. Validate input data
            // 2. Process the payment
            // 3. Create the order in the database
            // 4. Clear the shopping cart

            // For now, we'll just redirect to the confirmation page with the order ID
            // Simulating a new order with ID 1001
            return RedirectToAction("OrderConfirmation", new { orderId = 1001 });
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            // In a real application, we would:
            // 1. Fetch the order details from the database using the orderId
            // 2. Pass the order details to the view

            // For this demo, we'll create a mock order
            var orderDetails = new
            {
                OrderId = orderId,
                OrderDate = DateTime.Now,
                ShippingAddress = "123 Main St, City, Country",
                Items = new List<ShoppingCart>
                {
                    new ShoppingCart
                    {
                        Id = 1,
                        Name = "High-Performance Laptop",
                        Description = "16GB RAM, 512GB SSD, Intel Core i7",
                        Price = 999.99,
                        Count = 1,
                        Image = "https://tse4.mm.bing.net/th/id/OIP.okPHK-lOk_E5nzOZsGx2dwHaFI?cb=iwc2&rs=1&pid=ImgDetMain"
                    },
                    new ShoppingCart
                    {
                        Id = 2,
                        Name = "Premium Smartphone",
                        Description = "128GB Storage, 8GB RAM, 108MP Camera",
                        Price = 799.99,
                        Count = 1,
                        Image = "https://purepng.com/public/uploads/large/purepng.com-mobile-phone-with-touchmobilemobile-phonehandymobile-devicetouchscreenmobile-phone-device-231519333033crymn.png"
                    }
                },
                TotalPrice = 999.99 + 799.99 // = 1799.98
            };

            return View(orderDetails);
        }
    }
}
