using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DataLayer.Entities;
using BusinessLayer.ViewModel.Profile;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace DepiProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPhotoUploadService _photoUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IPhotoUploadService photoUploadService,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _photoUploadService = photoUploadService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            
            var photoUrl = user.PhotoUrl;
            // If no photo URL exists, use the default
            if (string.IsNullOrEmpty(photoUrl))
            {
                photoUrl = "/images/profiles/default-profile.png";
            }

            var model = new ProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                PhotoUrl = photoUrl
            };

            return View(model);
        }

    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }
        
        var photoUrl = user.PhotoUrl;
        // If no photo URL exists, use the default
        if (string.IsNullOrEmpty(photoUrl))
        {
            photoUrl = "/images/profiles/default-profile.png";
        }

        var model = new ProfileViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            City = user.City,
            State = user.State,
            PostalCode = user.PostalCode,
            PhotoUrl = photoUrl
        };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditProfile", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.State = model.State;
            user.PostalCode = model.PostalCode;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("EditProfile", model);
            }

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Index));
        }        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhoto(ProfileViewModel model)
        {
            if (model.PhotoFile == null || model.PhotoFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a photo.";
                return RedirectToAction(nameof(Index));
            }
            
            // Validate image type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(model.PhotoFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "Only JPG, PNG and GIF image formats are allowed.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                string photoUrl;

                // Use Cloudinary if configured
                try
                {
                    photoUrl = await _photoUploadService.UploadPhotoAsync(model.PhotoFile);
                }
                catch
                {
                    // Fallback to local storage if Cloudinary upload fails
                    photoUrl = await SavePhotoLocally(model.PhotoFile);
                }                user.PhotoUrl = photoUrl;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Profile photo updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating photo: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
        private async Task<string> SavePhotoLocally(Microsoft.AspNetCore.Http.IFormFile photo)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
            
            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Create unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
            }

            // Return the relative path
            return "/images/profiles/" + uniqueFileName;
        }
    }
}