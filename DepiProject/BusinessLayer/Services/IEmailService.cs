using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlMessage);
    Task SendConfirmationEmailAsync(string to, string username, string confirmationLink);
    Task SendPasswordResetEmailAsync(string to, string username, string resetLink);
}
