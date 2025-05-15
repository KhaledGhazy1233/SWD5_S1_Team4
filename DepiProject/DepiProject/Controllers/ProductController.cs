using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Product;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DepiProject.Controllers;

public class ProductController : Controller
{
    #region   Fields
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    #endregion
    // Sample data for demonstration - in a real app this would come from a database
    private static List<Product> _products = new List<Product>
        {            // Laptops
            new Product {
                ProductId = 1,
                Name = "TechBook Pro",
                Description = "Powerful laptop with high-performance features for professionals.",
                Price = 1299.99m,
                IsAvailable = true,
                //ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.okPHK-lOk_E5nzOZsGx2dwHaFI?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 1,
                Brand = "TechXpress",
                Model = "TB-2023",
                TechnicalSpecifications = "Intel Core i7, 16GB RAM, 512GB SSD, 15.6\" 4K Display",
                DiscountPercentage = 5,
                IsFeatured = true,
                WarrantyInfo = "2 Years Limited Warranty",
                StockQuantity = 15
            },
            new Product {
              ProductId = 2,
                Name = "GamerStation X",
                Description = "Ultimate gaming laptop with top-tier graphics and cooling system.",
                Price = 1899.99m,
                IsAvailable = true,
              //  ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.okPHK-lOk_E5nzOZsGx2dwHaFI?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 1,
                Brand = "TechXpress",
                Model = "GS-X500",
                TechnicalSpecifications = "AMD Ryzen 9, 32GB RAM, 1TB SSD, NVIDIA RTX 3080, 17.3\" 144Hz Display",
                DiscountPercentage = 0,
                IsFeatured = true,
                WarrantyInfo = "3 Years Premium Warranty",
                StockQuantity = 8
            },
              // Mobiles
            new Product {
             ProductId = 3,
                Name = "UltraPhone 12",
                Description = "Flagship smartphone with advanced camera system and all-day battery.",
                Price = 899.99m,
                IsAvailable = true,
              //  ImageUrl = "https://purepng.com/public/uploads/large/purepng.com-mobile-phone-with-touchmobilemobile-phonehandymobile-devicetouchscreenmobile-phone-device-231519333033crymn.png",
                CategoryId = 2,
                Brand = "UltraTech",
                Model = "UP-12",
                TechnicalSpecifications = "6.7\" OLED Display, 12GB RAM, 256GB Storage, 5000mAh Battery",
                DiscountPercentage = 10,
                IsFeatured = true,
                WarrantyInfo = "1 Year Manufacturer Warranty",
                StockQuantity = 25
            },
            new Product {
             ProductId= 4,
                Name = "SlimTab Pro",
                Description = "Ultra-thin tablet perfect for productivity and entertainment on the go.",
                Price = 649.99m,
                IsAvailable = true,
              //  ImageUrl = "https://purepng.com/public/uploads/large/purepng.com-mobile-phone-with-touchmobilemobile-phonehandymobile-devicetouchscreenmobile-phone-device-231519333033crymn.png",
                CategoryId = 2,
                Brand = "SlimTech",
                Model = "ST-P10",
                TechnicalSpecifications = "10.5\" Retina Display, 8GB RAM, 128GB Storage, 12MP Camera",
                DiscountPercentage = 0,
                IsFeatured = false,
                WarrantyInfo = "1 Year Limited Warranty",
                StockQuantity = 12
            },
            
            // Cameras
            new Product {
          ProductId = 5,
                Name = "ProShot DSLR X200",                Description = "Professional DSLR camera with exceptional image quality and versatile lens options.",
                Price = 1499.99m,
                IsAvailable = true,
           //     ImageUrl = "https://tse3.mm.bing.net/th/id/OIP.Kuw2tT6BMjFfN3UpgO1j2QHaE8?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 3,
                Brand = "ProShot",
                Model = "X200",
                TechnicalSpecifications = "24.2MP Full-Frame Sensor, 4K Video, 45-Point AF System, ISO 100-25600",
                DiscountPercentage = 7,
                IsFeatured = true,
                WarrantyInfo = "2 Years International Warranty",
                StockQuantity = 5
            },
            new Product {
             ProductId = 6,
                Name = "MirrorLite M50",
                Description = "Compact mirrorless camera offering the perfect balance of quality and portability.",
                Price = 799.99m,
                IsAvailable = true,
            //    ImageUrl = "https://tse3.mm.bing.net/th/id/OIP.Kuw2tT6BMjFfN3UpgO1j2QHaE8?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 3,
                Brand = "CameraElite",
                Model = "ML-M50",
                TechnicalSpecifications = "20.1MP APS-C Sensor, 4K/30p Video, 3\" Vari-angle Touchscreen",
                DiscountPercentage = 0,
                IsFeatured = false,
                WarrantyInfo = "1 Year Manufacturer Warranty",
                StockQuantity = 10
            }
        };

    #region   Constructor
    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }
    #endregion
    // Sample data for demonstration - in a real app this would come from a database

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
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _productService.GetUpdateProductVmById(id);
        if (vm == null)
            return RedirectToAction("Categories", "Admin");


        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(GetProductEditVm Vm)
    {
        if (!ModelState.IsValid)
            return View(Vm);

        //var result = await _productService.UpdateAsync(Vm);

        //if (result == "Success")
        //    return RedirectToAction("Categories", "Admin");


        //TempData["ErrorMessage"] = result;
        return View(Vm);
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