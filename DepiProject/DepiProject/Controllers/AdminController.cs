using BusinessLayer.Constants;
using BusinessLayer.Services.Interface;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DepiProject.Controllers;

[Authorize(Roles = Roles.Admin)]
public class AdminController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AdminController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public AdminController(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AdminController> logger,
        ICategoryService categoryService,
        IProductService productService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _categoryService = categoryService;
        _productService = productService;
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

            // Get real statistics for dashboard from database
            var totalProducts = _unitOfWork.Product.GetAll().Count();
            var totalCategories = _unitOfWork.Category.GetAll().Count();
            var totalOrders = _unitOfWork.Orders.GetAll().Count();

            // Get customer count - assumes users with Customer role
            var customerUsers = await _userManager.GetUsersInRoleAsync(Roles.Customer);
            var totalCustomers = customerUsers.Count;

            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalCustomers = totalCustomers;

            // Administrator info
            ViewBag.UserName = user.UserName;
            ViewBag.Email = user.Email;
            ViewBag.JoinDate = user.Created;

            _logger.LogInformation("Admin {UserName} accessed the dashboard", user.UserName);

            // Get recent orders from database
            var recentOrders = _unitOfWork.Orders.GetAll(includeProperties: "ApplicationUser")
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .Select(o => new
                {
                    Id = $"ORD-{o.OrderId}",
                    Customer = o.Name,
                    Date = o.CreatedAt,
                    Total = o.FinalPrice,
                    Status = o.IsDeleted ? "Cancelled" : "Delivered"
                })
                .ToList();

            ViewBag.RecentOrders = recentOrders;

            // Get top selling products (top 3 by sales)
            // This is a simplified example - in a real application you would calculate this from order details
            var topProducts = _unitOfWork.Product.GetAll()
                .Take(3)
                .Select(p => new
                {
                    Name = p.Name,
                    Sales = new Random().Next(10, 50), // Example placeholder - replace with real data
                    Revenue = new Random().Next(10000, 40000) / 100.0m // Example placeholder - replace with real data
                })
                .ToList();

            ViewBag.TopProducts = topProducts;
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

            //var electronicsCategory = new DepiProject.Models.Category { Id = 1, Name = "Electronics" };
            //var clothingCategory = new DepiProject.Models.Category { Id = 2, Name = "Clothing" };
            //var booksCategory = new DepiProject.Models.Category { Id = 3, Name = "Books" };

            var result = _productService.GetProductsVm();
            return View(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Products action: {ErrorMessage}", ex.Message);
            TempData["ErrorMessage"] = "There was an error loading the products. Please try again.";
            return RedirectToAction("Dashboard");
        }
    }

    public async Task<IActionResult> Categories(int pageNumber, int pageSize)
    {
        try
        {
            _logger.LogInformation("Admin accessed the static categories page");
            //if (pageNumber == 0 || pageSize == 0)
            //{
            //    pageNumber = 1;
            //    pageSize = 5;
            //}

            //var categories = await _categoryService.GetPaginatedCategories(pageNumber, pageSize);
            var categories = await _categoryService.GetAllCategories();
            return View(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Categories action: {ErrorMessage}", ex.Message);
            TempData["ErrorMessage"] = "There was an error loading the categories. Please try again.";
            return RedirectToAction("Dashboard");
        }
    }
    public IActionResult Orders()
    {
        try
        {
            // Get orders from the database
            var orders = _unitOfWork.Orders.GetAll(includeProperties: "ApplicationUser")
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            _logger.LogInformation("Admin accessed orders page with {OrderCount} orders", orders.Count);

            return View(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Orders action: {ErrorMessage}", ex.Message);
            TempData["ErrorMessage"] = "There was an error loading the orders. Please try again.";
            return RedirectToAction("Dashboard");
        }
    }
    [HttpGet]
    public IActionResult GetOrderDetails(int id)
    {
        try
        {
            var order = _unitOfWork.Orders.Get(o => o.OrderId == id, includeProperties: "ApplicationUser,ProductOrders");

            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }

            // Create a dynamic object to ensure property names are camelCase for JavaScript
            var orderDetails = new
            {
                orderId = order.OrderId,
                numberOfProduct = order.NumberOfProduct,
                totalPrice = order.TotalPrice,
                discount = order.Discount,
                fax = order.Fax,
                finalPrice = order.FinalPrice,
                addressDelivered = order.AddressDelivered,
                createdAt = order.CreatedAt,
                isDeleted = order.IsDeleted,
                phoneNumber = order.PhoneNumber,
                streetAddress = order.StreetAddress,
                city = order.City,
                state = order.State,
                postalCode = order.PostalCode,
                name = order.Name,
                applicationUser = new
                {
                    id = order.ApplicationUser?.Id,
                    email = order.ApplicationUser?.Email,
                    name = order.ApplicationUser?.UserName
                },
                productOrders = order.ProductOrders.Select(po => new
                {
                    productOrderId = po.ProductOrderId,
                    name = po.Name,
                    price = po.Price,
                    amount = po.Amount,
                    brand = po.Brand
                }).ToList()
            };

            return Json(new { success = true, data = orderDetails });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order details for ID {OrderId}: {ErrorMessage}", id, ex.Message);
            return Json(new { success = false, message = "An error occurred while retrieving order details" });
        }
    }
    [HttpPost]
    public IActionResult UpdateOrderStatus(int id, string status)
    {
        try
        {
            _logger.LogInformation("Attempting to update order {OrderId} to status {Status}", id, status);

            var order = _unitOfWork.Orders.Get(o => o.OrderId == id);

            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }

            // Update order status based on status parameter
            if (status == "Cancelled")
            {
                order.IsDeleted = true;
                _logger.LogInformation("Setting order {OrderId} as Cancelled (IsDeleted=true)", id);
            }
            else
            {
                order.IsDeleted = false;
                // In a real application, you'd likely have a more complex status system
                // e.g., order.Status = status;
                _logger.LogInformation("Setting order {OrderId} as active (IsDeleted=false), Status: {Status}", id, status);
            }

            _unitOfWork.Orders.Update(order);
            _unitOfWork.Save();

            _logger.LogInformation("Successfully updated order {OrderId} status to {Status}", id, status);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status for ID {OrderId}: {ErrorMessage}", id, ex.Message);
            return Json(new { success = false, message = "An error occurred while updating order status" });
        }
    }

    [HttpPost]
    public IActionResult CancelOrder(int id)
    {
        try
        {
            var order = _unitOfWork.Orders.Get(o => o.OrderId == id);

            if (order == null)
            {
                return Json(new { success = false, message = "Order not found" });
            }

            // Mark the order as deleted (cancelled) instead of actually removing it
            order.IsDeleted = true;

            _unitOfWork.Orders.Update(order);
            _unitOfWork.Save();

            _logger.LogInformation("Order {OrderId} cancelled", id);

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling order ID {OrderId}: {ErrorMessage}", id, ex.Message);
            return Json(new { success = false, message = "An error occurred while cancelling the order" });
        }
    }
}
