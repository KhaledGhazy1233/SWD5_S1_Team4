using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    public class ProductController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
