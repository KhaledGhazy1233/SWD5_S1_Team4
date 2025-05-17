using BusinessLayer.Constants;
using BusinessLayer.Services;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using DepiProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace DepiProject.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailService,
        IConfiguration configuration,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "You must confirm your email before logging in.");
                ViewData["ShowResendLink"] = true;
                ViewData["Email"] = model.Email;
                return View(model);
            }

            if (user != null && !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Your account is not active. Please confirm your email.");
                ViewData["ShowResendLink"] = true;
                ViewData["Email"] = model.Email;
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["success"] = "Login successful";

                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, Roles.Admin))
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (await _userManager.IsInRoleAsync(user, Roles.Customer))
                    {
                        return RedirectToAction("Dashboard", "Customer");
                    }
                }

                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else if (result.IsNotAllowed)
            {
                var userAccount = await _userManager.FindByEmailAsync(model.Email);
                if (userAccount != null && !await _userManager.IsEmailConfirmedAsync(userAccount))
                {
                    ModelState.AddModelError(string.Empty, "You must confirm your email before logging in.");
                    ViewData["ShowResendLink"] = true;
                    return View(model);
                }
                ModelState.AddModelError(string.Empty, "Login not allowed.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        return View(model);
    }

    // GET: /Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Created = DateTime.Now,
                IsActive = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Customer);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var appBaseUrl = _configuration["AppUrl"] ??
                    $"{Request.Scheme}://{Request.Host}";
                var confirmationLink = $"{appBaseUrl}/Account/ConfirmEmail?userId={user.Id}&token={token}";

                try
                {
                    await _emailService.SendConfirmationEmailAsync(
                        user.Email,
                        user.UserName,
                        confirmationLink);
                    TempData["success"] = "Registration successful! Please check your email to confirm your account.";
                }
                catch (Exception ex)
                {
                    // Log error but don't show to user
                }

                TempData["Email"] = user.Email;
                return RedirectToAction(nameof(RegisterConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        TempData["success"] = "You have been logged out successfully";
        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/AccessDenied
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // GET: /Account/ForgotPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // POST: /Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // No reveal for security
                return View("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var appBaseUrl = _configuration["AppUrl"] ??
                $"{Request.Scheme}://{Request.Host}";
            var email = user.Email ?? "";
            var resetLink = $"{appBaseUrl}/Account/ResetPassword?email={Uri.EscapeDataString(email)}&token={token}";

            try
            {
                await _emailService.SendPasswordResetEmailAsync(
                    user.Email ?? "",
                    user.UserName ?? "",
                    resetLink);

                TempData["success"] = "Password reset email sent. Please check your inbox.";
            }
            catch (Exception ex)
            {
                // Log exception
            }

            return View("ForgotPasswordConfirmation");
        }

        return View(model);
    }

    // GET: /Account/ResetPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string email = null, string token = null)
    {
        if (email == null || token == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid password reset token.");
            return View();
        }

        var model = new ResetPasswordViewModel
        {
            Email = email,
            Code = token
        };

        return View(model);
    }

    // POST: /Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // Don't reveal
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        try
        {
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);

            if (result.Succeeded)
            {
                TempData["success"] = "Your password has been reset successfully.";
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while resetting your password.");
        }

        return View(model);
    }

    // GET: /Account/ResetPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    // GET: /Account/ConfirmEmail
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        try
        {
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);

                TempData["success"] = "Thank you for confirming your email. Your account is now active.";
                return View("ConfirmEmailSuccess");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                ModelState.AddModelError(string.Empty, "Error confirming your email. Please try again or request a new confirmation link.");
                return View("Error");
            }
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Invalid email confirmation link.");
            return View("Error");
        }
    }

    // GET: /Account/RegisterConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult RegisterConfirmation()
    {
        if (TempData["Email"] != null)
        {
            var emailValue = TempData["Email"];
            if (emailValue != null)
            {
                ViewData["Email"] = emailValue.ToString();
            }
        }
        return View();
    }

    // GET: /Account/ResendEmailConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResendEmailConfirmation(string email = null)
    {
        var model = new ResendEmailConfirmationViewModel();
        if (!string.IsNullOrEmpty(email))
        {
            model.Email = email;
        }
        return View(model);
    }

    // POST: /Account/ResendEmailConfirmation
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return View(model);
        }

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (isEmailConfirmed)
        {
            ModelState.AddModelError(string.Empty, "This email is already confirmed. Please try logging in.");
            return View(model);
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var appBaseUrl = _configuration["AppUrl"] ??
            $"{Request.Scheme}://{Request.Host}";
        var confirmationLink = $"{appBaseUrl}/Account/ConfirmEmail?userId={user.Id}&token={token}";

        try
        {
            await _emailService.SendConfirmationEmailAsync(
                user.Email ?? "",
                user.UserName ?? "",
                confirmationLink);

            TempData["success"] = "Verification email sent. Please check your email.";
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Error sending email. Please try again later.");
            return View(model);
        }

        return RedirectToAction(nameof(RegisterConfirmation));
    }

    // POST: /Account/ChangePassword
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
        {
            return Json(new { success = false, message = "All fields are required." });
        }

        if (newPassword != confirmPassword)
        {
            return Json(new { success = false, message = "New password and confirmation do not match." });
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Json(new { success = false, message = "User not found." });
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!changePasswordResult.Succeeded)
        {
            var errors = string.Join(", ", changePasswordResult.Errors.Select(e => e.Description));
            return Json(new { success = false, message = errors });
        }

        await _signInManager.RefreshSignInAsync(user);
        return Json(new { success = true, message = "Your password has been changed successfully." });
    }

    // Helper method
    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
}