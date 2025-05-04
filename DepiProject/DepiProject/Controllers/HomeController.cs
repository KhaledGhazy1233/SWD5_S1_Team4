using DepiProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DepiProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult index()
        {
            return View();
        }

        public IActionResult shop()
        {
            return View();
        }

        public IActionResult shopsingle(int id)
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }

        public IActionResult about()
        {
            return View();
        }

        public IActionResult admin()
        {
            return View();
        }
    }
}
