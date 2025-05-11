using DepiProject.Models;
using DepiProject.ViewModel;
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
            // In a real application, this would:
            // 1. Process the payment
            // 2. Create the order in the database
            // 3. Clear the shopping cart

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
                TotalPrice = 40.0
            };

            return View(orderDetails);
        }
    }
}
