using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Category;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace DepiProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _categoryService.Create(vm);

            if (result == "Success")
                return RedirectToAction("Index");

            TempData["ErrorMessage"] = result;
            return View(vm);
        }
        // Service  take id  return update cat vm
        /* public async Task<IActionResult> Edit(int id)
         {
             var category = await _categoryService.GetCategoryById(id);
             if (category == null)
                 return RedirectToAction("Index");

             var vm = new UpdateCategoryVm
             {
                 Id = category.CategoryId,
                 Name = category.Name,
                 Description = category.Description
             };

             return View(vm);
         }*/
        // Last-----------------------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _categoryService.GetUpdateCategoryVmById(id);
            if (vm == null)
                return RedirectToAction("Index");

            return View(vm);
        }
        //----------------------------------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _categoryService.Update(vm);

            if (result == "Success")
                return RedirectToAction("Index");

            TempData["ErrorMessage"] = result;
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.Delete(id);
            TempData["Message"] = result;
            return RedirectToAction("Index");
        }
    }
}
