namespace DepiProject.ViewModels
{
    public class LoginStatusViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCustomer { get; set; }
        public int CartItemCount { get; set; }
    }
}
