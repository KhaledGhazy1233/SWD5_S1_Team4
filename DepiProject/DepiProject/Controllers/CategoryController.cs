using DepiProject.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddCategoryVm vm)
        {
            return View();
        }
    }
}
