using BusinessLayer.Constants;
using BusinessLayer.Services.Interface;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using DepiProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DepiProject.Controllers;

public class HomeController : Controller
{    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ISharedService _sharedService;
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ISharedService sharedService,
        IProductService productService,
        ICategoryService categoryService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _sharedService = sharedService;
        _productService = productService;
        _categoryService = categoryService;
        _unitOfWork = unitOfWork;
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public async Task<IActionResult> Index()
    {
        try
        {
            _logger.LogInformation("Home page accessed");


            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    ViewBag.UserName = user.UserName;

                    if (await _userManager.IsInRoleAsync(user, Roles.Admin))
                    {
                        ViewBag.UserRole = Roles.Admin;
                        ViewBag.RoleBadgeClass = "bg-primary";
                    }
                    else if (await _userManager.IsInRoleAsync(user, Roles.Customer))
                    {                        ViewBag.UserRole = Roles.Customer;
                        ViewBag.RoleBadgeClass = "bg-success";

                        // Get real order data for the customer
                        var orders = _unitOfWork.Orders.GetAll(o => o.ApplicationUserId == user.Id, includeProperties: "ProductOrders");
                        
                        // Count pending orders (non-cancelled orders)
                        var pendingOrdersCount = orders.Count(o => !o.IsDeleted);
                        ViewBag.PendingOrdersCount = pendingOrdersCount;
                        
                        // Get date of latest order
                        var latestOrder = orders.OrderByDescending(o => o.CreatedAt).FirstOrDefault();
                        ViewBag.LatestOrderDate = latestOrder != null 
                            ? latestOrder.CreatedAt.ToString("MMM dd, yyyy") 
                            : "N/A";
                    }
                }
            }

            var homeVm = _sharedService.GetHomeDetailsAsync();

            return View(homeVm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when loading home page: {Message}", ex.Message);
            TempData["ErrorMessage"] = "An error occurred while loading the page. Please try again.";
            return View();
        }
    }
    public async Task<IActionResult> Shop(string category, string search, string brand, decimal? minPrice, decimal? maxPrice)
    {
        try
        {
            _logger.LogInformation("Shop page accessed with category filter: {Category}", category ?? "All");

            List<Product> products = new List<Product>();

            // Get all categories for the filter
            var categories = await _categoryService.GetAllCategories();
            ViewBag.Categories = categories;

            if (!string.IsNullOrEmpty(category))
            {
                // Find the category ID by name
                var categoryObj = categories.FirstOrDefault(c => c.Name.ToLower() == category.ToLower());
                if (categoryObj != null)
                {
                    var categoryProducts = await _productService.GetProductByCategoryID(categoryObj.Id);

                    // Convert view model to Product (or modify your view to use the view model directly)
                    foreach (var product in categoryProducts)
                    {
                        var newProduct = new Product
                        {
                            ProductId = product.ProductId,
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            Brand = product.Brand,
                            DiscountPercentage = product.DiscountPercentage,
                            ProductImages = new List<ProductImage>()
                        };

                        if (!string.IsNullOrEmpty(product.ImageUrl))
                        {
                            newProduct.ProductImages.Add(new ProductImage { Path = product.ImageUrl });
                        }

                        products.Add(newProduct);
                    }

                    ViewBag.CurrentCategory = categoryObj.Name;
                }
            }
            else
            {
                // Get all featured products if no category is selected
                var allProducts = await _productService.GetFeaturedProduct();

                // Convert view model to Product (or modify your view to use the view model directly)
                foreach (var product in allProducts)
                {
                    var newProduct = new Product
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Brand = product.Brand,
                        DiscountPercentage = product.DiscountPercentage,
                        ProductImages = new List<ProductImage>()
                    };

                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        newProduct.ProductImages.Add(new ProductImage { Path = product.ImageUrl });
                    }

                    products.Add(newProduct);
                }
            }

            // Apply additional filters
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                               p.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
                                    .ToList();
            }

            if (!string.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            }

            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when loading shop page: {Message}", ex.Message);
            TempData["ErrorMessage"] = "An error occurred while loading the products. Please try again.";
            return View(new List<Product>());
        }
    }
    public IActionResult ShopSingle(int id)
    {
        try
        {
            _logger.LogInformation("Product detail page accessed for product ID: {ProductId}", id);

            var productDetails = _productService.GetProductDetailsVm(id);
            if (productDetails == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", id);
                return NotFound();
            }

            // Convert to Product model for view compatibility
            var product = new Product
            {
                ProductId = productDetails.ProductId,
                Name = productDetails.Name,
                Description = productDetails.Description,
                Price = productDetails.Price,
                StockQuantity = productDetails.StockQuantity,
                Brand = productDetails.Brand,
                Model = productDetails.Model,
                TechnicalSpecifications = productDetails.TechnicalSpecifications,
                DiscountPercentage = productDetails.DiscountPercentage,
                WarrantyInfo = productDetails.WarrantyInfo,
                ProductImages = new List<ProductImage>()
            };

            if (!string.IsNullOrEmpty(productDetails.ImageUrl))
            {
                product.ProductImages.Add(new ProductImage { Path = productDetails.ImageUrl });
            }

            // Get related products from the same category (using Featured Products for now)
            var featuredProducts = _productService.GetFeaturedProduct().Result;
            var relatedProducts = featuredProducts
                .Where(p => p.ProductId != id)
                .Take(4)
                .Select(p =>
                {
                    var relatedProduct = new Product
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Brand = p.Brand,
                        DiscountPercentage = p.DiscountPercentage,
                        ProductImages = new List<ProductImage>()
                    };

                    if (!string.IsNullOrEmpty(p.ImageUrl))
                    {
                        relatedProduct.ProductImages.Add(new ProductImage { Path = p.ImageUrl });
                    }

                    return relatedProduct;
                })
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when loading product details for ID {ProductId}: {Message}", id, ex.Message);
            TempData["ErrorMessage"] = "An error occurred while loading the product details. Please try again.";
            return RedirectToAction("Shop");
        }
    }
    public IActionResult Contact()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }
}