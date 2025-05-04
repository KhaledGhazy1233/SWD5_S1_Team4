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
                ShoppingCart = new List<CartItem>
    {
        new CartItem
        {
            Id = 1,
            Name = "Product 1",
            Description = "Description of Product 1",
            Price = 10.0,
            Count = 2,
            Image = "image1.jpg"
        },
        new CartItem
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
    }
}
