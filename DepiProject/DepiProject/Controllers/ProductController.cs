using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DepiProject.Controllers;

public class ProductController : Controller
{
    #region Fields
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<ProductController> _logger;
    #endregion
    
    #region Constructor
    public ProductController(
        IProductService productService, 
        ICategoryService categoryService, 
        ILogger<ProductController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _logger = logger;
    }
    #endregion

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(int id)
    {
        var product = _productService.GetProductDetailsVm(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Get products by category
    public async Task<IActionResult> ByCategory(int categoryId)
    {
        var products = await _productService.GetProductByCategoryID(categoryId);
        return View(products);
    }

    // Get featured products
    public async Task<IActionResult> Featured()
    {
        var products = await _productService.GetFeaturedProduct();
        return View(products);
    }




    public async Task<IActionResult> Create()
    {
        var result = _categoryService.GetDropDown();

        ViewData["categories"] = result is not null ? new SelectList(result, "Id", "Name") : null;
        return View(new CreateProductVm());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await _productService.CreateAsync(vm);

        if (result == "Success")
            return RedirectToAction("Categories", "Admin");

        TempData["ErrorMessage"] = result;

        ViewData["categories"] = result is not null ? new SelectList(_categoryService.GetDropDown(), "Id", "Name") : null;
        return View(vm);
    }    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _productService.GetUpdateProductVmById(id);
        if (vm == null)
            return RedirectToAction("Categories", "Admin");

        return View(vm);
    }    [HttpPost]
    public async Task<IActionResult> Edit(GetProductEditVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);
        
        // Find the selected category ID
        int categoryId = 1;
        if (vm.categoryDropDowns != null && !string.IsNullOrEmpty(vm.CategoryName))
        {
            var category = vm.categoryDropDowns.FirstOrDefault(c => c.Name == vm.CategoryName);
            if (category != null)
            {
                categoryId = category.Id;
            }
        }
        
        var updateVm = new UpdateProductVm
        {
            ProductId = vm.ProductId,
            Id = vm.ProductId,
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            StockQuantity = vm.Amount,
            Brand = vm.Brand,
            CategoryId = categoryId,
            Model = vm.Brand + " Model",
            TechnicalSpecifications = "Updated from form",
            DiscountPercentage = 0,
            IsFeatured = true,
            WarrantyInfo = "Standard Warranty"
        };
        
        try
        {    
            var result = await _productService.UpdateAsync(updateVm);

            if (result == "Success")
            {
                TempData["SuccessMessage"] = "Product updated successfully";
                return RedirectToAction("Categories", "Admin");
            }

            TempData["ErrorMessage"] = result;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Error updating product: " + ex.Message;
            _logger.LogError(ex, "Error updating product {ProductId}", vm.ProductId);
        }
        
        return View(vm);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (result == "Success")
            return RedirectToAction("Products", "Admin");

        TempData["Message"] = result;
        return RedirectToAction("Products", "Admin");

    }
}