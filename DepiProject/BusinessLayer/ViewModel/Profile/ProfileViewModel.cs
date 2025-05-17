using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModel.Profile
{
    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        public string Address { get; set; } = string.Empty;
        
        public string City { get; set; } = string.Empty;
        
        public string State { get; set; } = string.Empty;
        
        public string PostalCode { get; set; } = string.Empty;
        
        public string? PhotoUrl { get; set; }
        
        // Property to hold the uploaded photo file
        [Display(Name = "Profile Photo")]
        public IFormFile? PhotoFile { get; set; }
    }
}
