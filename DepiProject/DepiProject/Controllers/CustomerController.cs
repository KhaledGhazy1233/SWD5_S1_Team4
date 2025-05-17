using BusinessLayer.Constants;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepiProject.Models;
using DepiProject.ViewModels;
using DataLayer.Repository.IRepository;

namespace DepiProject.Controllers;

[Authorize(Roles = Roles.Customer)]
public class CustomerController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CustomerController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerController(
        UserManager<ApplicationUser> userManager,
        ILogger<CustomerController> logger,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        ViewBag.IsCustomerArea = true;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogWarning("User attempted to access Customer Dashboard without being logged in");
            return RedirectToAction("Login", "Account", new { returnUrl = "/Customer/Dashboard" });
        }

        ViewBag.UserName = user.UserName;
        ViewBag.Email = user.Email;
        ViewBag.FullName = $"{user.FirstName} {user.LastName}";
        ViewBag.JoinDate = user.Created;

        // Get customer orders
        var orders = _unitOfWork.Orders.GetAll(o => o.ApplicationUserId == user.Id, "ProductOrders,ProductOrders.Product");

        // Calculate metrics from real orders
        ViewBag.TotalOrders = orders.Count();

        // Calculate total spent
        decimal totalSpent = 0;
        foreach (var order in orders)
        {
            totalSpent += order.FinalPrice;
        }
        ViewBag.TotalSpent = totalSpent;

        // Create a list of recent orders for display
        ViewBag.RecentOrders = orders
            .OrderByDescending(o => o.CreatedAt)
            .Take(3)
            .Select(o => new
            {
                OrderId = $"ORD-{o.OrderId}",
                Date = o.CreatedAt,
                Status = o.IsDeleted ? "Cancelled" : "Delivered", // This should be replaced with real status logic
                Total = o.FinalPrice
            })
            .ToList();

        // Get active categories for quick links
        ViewBag.Categories = _unitOfWork.Category.GetAll(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .Take(5) // Limit to 5 categories for the quick links
            .ToList();

        // Get active reward points (this would come from your rewards system)
        ViewBag.RewardPoints = 0; // Replace with actual rewards logic

        _logger.LogInformation("Customer {UserName} accessed the dashboard with full dynamic data", user.UserName);

        return View();
    }

    public async Task<IActionResult> Orders()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.UserName = user.UserName;
        ViewBag.Email = user.Email;

        // Get all orders for this customer
        var orders = _unitOfWork.Orders.GetAll(o => o.ApplicationUserId == user.Id, "ProductOrders")
            .Select(o => new ViewModels.OrderViewModel
            {
                Id = $"ORD-{o.OrderId}",
                RawId = o.OrderId,
                Date = o.CreatedAt,
                Status = o.IsDeleted ? "Cancelled" : "Delivered", // This should be replaced with real status logic
                Items = o.ProductOrders.Count(),
                Total = o.FinalPrice
            })
            .OrderByDescending(o => o.Date)
            .ToList();

        _logger.LogInformation("Customer {UserName} accessed their orders page with dynamic data", user.UserName);

        return View(orders);
    }

    public async Task<IActionResult> Wishlist()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.UserName = user.UserName;
        ViewBag.Email = user.Email;

        // Static wishlist data
        var wishlistItems = new List<object>
        {
            new {
                Id = 1,
                Name = "UltraPhone 12 Pro",
                ImageUrl = "/images/products/ultraphone.jpg",
                Price = 999.99m,
                InStock = true
            },
            new {
                Id = 2,
                Name = "TechBook Air",
                ImageUrl = "/images/products/techbook.jpg",
                Price = 1299.99m,
                InStock = true
            },
            new {
                Id = 3,
                Name = "SmartWatch Series 5",
                ImageUrl = "/images/products/smartwatch.jpg",
                Price = 299.99m,
                InStock = false
            },
            new {
                Id = 4,
                Name = "Wireless Headphones",
                ImageUrl = "/images/products/headphones.jpg",
                Price = 199.99m,
                InStock = true
            }
        };

        return View(wishlistItems);
    }

    public async Task<IActionResult> Cart()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.UserName = user.UserName;
        ViewBag.Email = user.Email;

        // Get real shopping cart data for the customer
        var cartItems = new ShoppingCartVM();
        var shoppingCartList = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == user.Id, "Product,Product.ProductImages");

        cartItems.ShoppingCartList = shoppingCartList.ToList();

        // Calculate the order total
        double orderTotal = 0;
        foreach (var item in shoppingCartList)
        {
            orderTotal += (double)(item.Price * item.Count);
        }
        cartItems.OrderTotal = orderTotal;

        _logger.LogInformation("Customer {UserName} accessed their dynamic cart data", user.UserName);

        return View(cartItems);
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userProfile = new
        {
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            City = user.City,
            State = user.State,
            PostalCode = user.PostalCode,
            JoinDate = user.Created
        };

        _logger.LogInformation("Customer {UserName} accessed their profile with dynamic user info", user.UserName);

        // Get dynamic statistics for the profile page
        var orders = _unitOfWork.Orders.GetAll(o => o.ApplicationUserId == user.Id);
        ViewBag.TotalOrders = orders.Count();
        ViewBag.CompletedOrders = orders.Count(o => !o.IsDeleted);
        ViewBag.PendingOrders = 0; // Replace with actual logic if you have pending status

        // Calculate total spent
        decimal totalSpent = 0;
        foreach (var order in orders)
        {
            totalSpent += order.FinalPrice;
        }
        ViewBag.TotalSpent = totalSpent;

        return View(userProfile);
    }

    public async Task<IActionResult> Settings()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.UserName = user.UserName;
        ViewBag.Email = user.Email;
        ViewBag.FullName = $"{user.FirstName} {user.LastName}";

        _logger.LogInformation("Customer {UserName} accessed settings with dynamic user info", user.UserName);

        // Static settings for the user
        var settings = new ViewModels.CustomerSettingsViewModel
        {
            ReceiveEmailNotifications = true,
            ReceiveSmsNotifications = false,
            NewsletterSubscribed = true,
            TwoFactorEnabled = false,
            Language = "English",
            Currency = "USD"
        };

        return View(settings);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateNotifications(ViewModels.CustomerSettingsViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Corn needs to change it or just remove it
        TempData["SuccessMessage"] = "Notification settings updated successfully!";

        _logger.LogInformation("Customer {UserName} updated notification settings", user.UserName);

        return RedirectToAction("Settings");
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePreferences(ViewModels.CustomerSettingsViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Corn needs to change it or just remove it
        TempData["SuccessMessage"] = "Preferences updated successfully!";

        _logger.LogInformation("Customer {UserName} updated preferences", user.UserName);

        return RedirectToAction("Settings");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSecurity(ViewModels.CustomerSettingsViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Corn needs to change it or just remove it
        TempData["SuccessMessage"] = "Security settings updated successfully!";

        _logger.LogInformation("Customer {UserName} updated security settings", user.UserName);

        return RedirectToAction("Settings");
    }

    public async Task<IActionResult> OrderDetails(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get the specific order
        var order = _unitOfWork.Orders.Get(o => o.OrderId == id && o.ApplicationUserId == user.Id, "ProductOrders,ProductOrders.Product");

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found";
            return RedirectToAction("Orders");
        }

        ViewBag.UserName = user.UserName;
        ViewBag.Email = user.Email;

        _logger.LogInformation("Customer {UserName} viewed order details for OrderID: {OrderId}", user.UserName, id);

        return View(order);
    }
}
