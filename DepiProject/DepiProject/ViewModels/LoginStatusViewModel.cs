using System;

namespace DepiProject.ViewModels
{
    public class LoginStatusViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string[] Roles { get; set; } = Array.Empty<string>();
        public bool IsAdmin { get; set; }
        public bool IsCustomer { get; set; }
        public int CartItemCount { get; set; }
    }
}
