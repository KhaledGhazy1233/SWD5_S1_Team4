using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}