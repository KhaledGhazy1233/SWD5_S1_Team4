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

namespace DepiProject.Controllers;

[Authorize(Roles = Roles.Customer)]
public class CustomerController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(
        UserManager<ApplicationUser> userManager,
        ILogger<CustomerController> logger)
    {
        _userManager = userManager;
        _logger = logger;
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
        
        _logger.LogInformation("Customer {UserName} accessed the dashboard with dynamic user info and static dashboard data", user.UserName);
        
        // Static data for dashboard
        ViewBag.TotalOrders = 5;
        ViewBag.WishlistItems = 12;
        ViewBag.TotalSpent = 1250.99m;
        ViewBag.ActiveVouchers = 2;
        
        // Static recent orders
        ViewBag.RecentOrders = new List<object> {
            new { 
                OrderId = "ORD-123456",
                Date = DateTime.Now.AddDays(-3),
                Status = "Delivered",
                Total = 299.99m
            },
            new {
                OrderId = "ORD-123457",
                Date = DateTime.Now.AddDays(-7),
                Status = "Shipped",
                Total = 549.99m
            },
            new {
                OrderId = "ORD-123458",
                Date = DateTime.Now.AddDays(-14),
                Status = "Delivered",
                Total = 199.99m
            }
        };
        
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
        
        // Static orders data
        var orders = new List<object>
        {
            new {
                Id = "ORD-123456",
                Date = DateTime.Now.AddDays(-3),
                Status = "Delivered",
                TrackingNumber = "TN-987654321",
                Items = 3,
                Total = 299.99m
            },
            new {
                Id = "ORD-123457",
                Date = DateTime.Now.AddDays(-7),
                Status = "Shipped",
                TrackingNumber = "TN-987654322",
                Items = 1,
                Total = 549.99m
            },
            new {
                Id = "ORD-123458",
                Date = DateTime.Now.AddDays(-14),
                Status = "Delivered",
                TrackingNumber = "TN-987654323",
                Items = 2,
                Total = 199.99m
            },
            new {
                Id = "ORD-123459",
                Date = DateTime.Now.AddDays(-30),
                Status = "Delivered",
                TrackingNumber = "TN-987654324",
                Items = 4,
                Total = 399.99m
            },
            new {
                Id = "ORD-123460",
                Date = DateTime.Now.AddDays(-45),
                Status = "Delivered",
                TrackingNumber = "TN-987654325",
                Items = 1,
                Total = 159.99m
            }
        };
        
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
        // Static cart data
        var cartItems = new ShoppingCartVM
        {
            ShoppingCartList = new List<DataLayer.Entities.ShoppingCart>
            {
                new DataLayer.Entities.ShoppingCart
                {
                    Id = 1,
                    ProductId = 1,
                    Count = 1,
                    Price = 999.99M,
                    Product = new DataLayer.Entities.Product
                    {
                        ProductId = 1,
                        Name = "UltraPhone 12 Pro",
                        Description = "Latest flagship smartphone with advanced camera system",
                        ProductImages = new List<DataLayer.Entities.ProductImage>
                        {
                            new DataLayer.Entities.ProductImage { Path = "/images/products/ultraphone.jpg" }
                        }
                    }
                },
                new DataLayer.Entities.ShoppingCart
                {
                    Id = 2,
                    ProductId = 2,
                    Count = 2,
                    Price = 149.99M,
                    Product = new DataLayer.Entities.Product
                    {
                        ProductId = 2,
                        Name = "Wireless Earbuds",
                        Description = "Premium noise-canceling wireless earbuds",
                        ProductImages = new List<DataLayer.Entities.ProductImage>
                        {
                            new DataLayer.Entities.ProductImage { Path = "/images/products/earbuds.jpg" }
                        }
                    }
                }
            },
            OrderTotal = 999.99 + (149.99 * 2) // = 1299.97
        };
        
        _logger.LogInformation("Customer {UserName} accessed their static cart with dynamic user info", user.UserName);
        
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
        
        // Static statistics for the profile page
        ViewBag.TotalOrders = 5;
        ViewBag.CompletedOrders = 4;
        ViewBag.PendingOrders = 1;
        ViewBag.TotalSpent = 1250.99m;
        
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
}
