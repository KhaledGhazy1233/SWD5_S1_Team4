using BusinessLayer.Constants;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepiProject.Controllers;

[Authorize(Roles = Roles.Admin)]
public class AdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AdminController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AdminController(
        IUnitOfWork unitOfWork, 
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AdminController> logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        ViewBag.IsAdminArea = true;
    }
    
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                _logger.LogWarning("User is authenticated but could not be found in the database");
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account", new { returnUrl = "/Admin/Dashboard" });
            }
            
            // Get statistics for dashboard from database
            ViewBag.TotalProducts = 10;
            ViewBag.TotalCategories = 3;
            ViewBag.TotalOrders = 25;
            ViewBag.TotalCustomers = 50;
            
            // Administrator info
            ViewBag.UserName = user.UserName;
            ViewBag.Email = user.Email;
            ViewBag.JoinDate = user.Created;
            
            _logger.LogInformation("Admin {UserName} accessed the dashboard", user.UserName);
            
            // Demo Orders (static data)
            ViewBag.RecentOrders = new List<object> {
            new { 
                Id = "ORD-001", 
                Customer = "John Doe", 
                Date = DateTime.Now.AddDays(-1),
                Total = 1299.99m,
                Status = "Completed"
            },
            new { 
                Id = "ORD-002", 
                Customer = "Jane Smith", 
                Date = DateTime.Now.AddDays(-2),
                Total = 899.99m,
                Status = "Processing"
            },
            new { 
                Id = "ORD-003", 
                Customer = "Mike Johnson", 
                Date = DateTime.Now.AddDays(-3),
                Total = 2199.99m,
                Status = "Pending"
            },
            };
        
            // Top Selling (Demo)
            ViewBag.TopProducts = new List<object> {
                new {
                    Name = "UltraPhone 12",
                    Sales = 42,
                    Revenue = 37799.58m
                },
                new {
                    Name = "TechBook Pro",
                    Sales = 28,
                    Revenue = 36399.72m
                },
                new {
                    Name = "ProShot DSLR X200",
                    Sales = 15,
                    Revenue = 22499.85m
                }
            };
        
            ViewBag.RoleBasedCss = true;
        
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Admin Dashboard: {ErrorMessage}", ex.Message);
            TempData["ErrorMessage"] = "There was an error loading the dashboard. Please try again.";
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult Products()
    {
        try
        {
            _logger.LogInformation("Admin accessed the static products page");
            
            var electronicsCategory = new DepiProject.Models.Category { Id = 1, Name = "Electronics" };
            var clothingCategory = new DepiProject.Models.Category { Id = 2, Name = "Clothing" };
            var booksCategory = new DepiProject.Models.Category { Id = 3, Name = "Books" };
            
            var products = new List<DepiProject.Models.Product>
            {
                new DepiProject.Models.Product {
                    Id = 1,
                    Name = "UltraPhone 12",
                    Description = "Latest smartphone with advanced features",
                    Price = 899.99m,
                    IsAvailable = true,
                    CategoryId = 1,
                    Category = electronicsCategory,
                    Brand = "TechCo",
                    StockQuantity = 25,
                    ImageUrl = "/images/products/phone.jpg"
                },
                new DepiProject.Models.Product {
                    Id = 2,
                    Name = "TechBook Pro",
                    Description = "High-performance laptop for professionals",
                    Price = 1299.99m,
                    IsAvailable = true,
                    CategoryId = 1,
                    Category = electronicsCategory,
                    Brand = "TechCo",
                    StockQuantity = 15,
                    ImageUrl = "/images/products/laptop.jpg"
                },
                new DepiProject.Models.Product {
                    Id = 3,
                    Name = "Casual T-Shirt",
                    Description = "Comfortable cotton t-shirt",
                    Price = 24.99m,
                    IsAvailable = true,
                    CategoryId = 2,
                    Category = clothingCategory,
                    Brand = "FashionStyle",
                    StockQuantity = 100,
                    ImageUrl = "/images/products/tshirt.jpg"
                },
                new DepiProject.Models.Product {
                    Id = 4,
                    Name = "Programming Guide",
                    Description = "Complete guide to modern programming",
                    Price = 39.99m,
                    IsAvailable = true,
                    CategoryId = 3,
                    Category = booksCategory,
                    Brand = "Tech Publishing",
                    StockQuantity = 50,
                    ImageUrl = "/images/products/book.jpg"
                },
                new DepiProject.Models.Product {
                    Id = 5,
                    Name = "ProShot DSLR X200",
                    Description = "Professional DSLR Camera with 4K video",
                    Price = 1499.99m,
                    IsAvailable = true,
                    CategoryId = 1,
                    Category = electronicsCategory,
                    Brand = "ProImage",
                    StockQuantity = 10,
                    ImageUrl = "/images/products/camera.jpg"
                }
            };
            
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Products action: {ErrorMessage}", ex.Message);
            TempData["ErrorMessage"] = "There was an error loading the products. Please try again.";
            return RedirectToAction("Dashboard");
        }
    }

    public IActionResult Categories()
    {
        try
        {
            _logger.LogInformation("Admin accessed the static categories page");
            
            var categories = new List<DepiProject.Models.Category>
            {
                new DepiProject.Models.Category { Id = 1, Name = "Electronics", SubCategory = "" },
                new DepiProject.Models.Category { Id = 2, Name = "Clothing", SubCategory = "" },
                new DepiProject.Models.Category { Id = 3, Name = "Books", SubCategory = "" },
                new DepiProject.Models.Category { Id = 4, Name = "Home & Garden", SubCategory = "" },
                new DepiProject.Models.Category { Id = 5, Name = "Sports", SubCategory = "" }
            };
            
            return View(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Categories action: {ErrorMessage}", ex.Message);
            TempData["ErrorMessage"] = "There was an error loading the categories. Please try again.";
            return RedirectToAction("Dashboard");
        }
    }
}
