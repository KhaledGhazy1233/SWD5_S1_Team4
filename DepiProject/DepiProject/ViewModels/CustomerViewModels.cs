using System.ComponentModel.DataAnnotations;

namespace DepiProject.ViewModels
{
    public class CustomerSettingsViewModel
    {
        public bool ReceiveEmailNotifications { get; set; }
        public bool ReceiveSmsNotifications { get; set; }
        public bool NewsletterSubscribed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string Language { get; set; } = "English";
        public string Currency { get; set; } = "USD";
    }
}
