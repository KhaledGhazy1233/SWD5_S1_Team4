using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Category;
using Microsoft.AspNetCore.Mvc;

namespace DepiProject.Controllers;

public class CategoryController : Controller
{
    #region   Fields
    private readonly ICategoryService _categoryService;
    #endregion

    #region    Constructor
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    #endregion

    #region    Methods
    #endregion
    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryVm vm)
    {
        if (!ModelState.IsValid)
            return View();

        var response = await _categoryService.Create(vm);

        if (response == "Success")
            RedirectToAction("Index");

        // display message in response
        return View();
    }
}
