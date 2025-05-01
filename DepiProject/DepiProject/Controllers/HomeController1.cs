using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
