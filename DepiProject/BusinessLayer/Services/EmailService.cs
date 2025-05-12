using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BusinessLayer.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _fromEmail = _configuration["EmailSettings:FromEmail"] ?? "default@example.com";
        _fromName = _configuration["EmailSettings:FromName"] ?? "TechXpress";
        _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.example.com";
        _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
        _smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "username";
        _smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "password";
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(to));

            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
            };

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            // Log the exception
            throw;
        }
    }

    public async Task SendConfirmationEmailAsync(string to, string username, string confirmationLink)
    {
        var subject = "TechXpress - Confirm Your Email";
        var htmlMessage = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background-color: #4CAF50; padding: 20px; text-align: center;'>
                        <h1 style='color: white;'>TechXpress</h1>
                    </div>
                    <div style='padding: 20px; border: 1px solid #ddd; border-top: none;'>
                        <h2>Hello {username},</h2>
                        <p>Thank you for registering at TechXpress! Please confirm your email address by clicking the button below:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{confirmationLink}' style='background-color: #4CAF50; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; font-weight: bold;'>Confirm Email</a>
                        </div>
                        <p>If the button doesn't work, you can also copy and paste the following link in your browser:</p>
                        <p><a href='{confirmationLink}'>{confirmationLink}</a></p>
                        <p>If you did not create an account, you can simply ignore this email.</p>
                        <p>Best regards,<br/>TechXpress Team</p>
                    </div>
                    <div style='background-color: #f1f1f1; padding: 10px; text-align: center; font-size: 12px; color: #666;'>
                        <p>&copy; {DateTime.Now.Year} TechXpress. All rights reserved.</p>
                    </div>
                </div>";

        await SendEmailAsync(to, subject, htmlMessage);
    }

    public async Task SendPasswordResetEmailAsync(string to, string username, string resetLink)
    {
        var subject = "TechXpress - Reset Your Password";
        var htmlMessage = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background-color: #4CAF50; padding: 20px; text-align: center;'>
                        <h1 style='color: white;'>TechXpress</h1>
                    </div>
                    <div style='padding: 20px; border: 1px solid #ddd; border-top: none;'>
                        <h2>Hello {username},</h2>
                        <p>We received a request to reset your password. Click the button below to set a new password:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{resetLink}' style='background-color: #4CAF50; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; font-weight: bold;'>Reset Password</a>
                        </div>
                        <p>If the button doesn't work, you can also copy and paste the following link in your browser:</p>
                        <p><a href='{resetLink}'>{resetLink}</a></p>
                        <p>If you did not request a password reset, you can safely ignore this email.</p>
                        <p>Best regards,<br/>TechXpress Team</p>
                    </div>
                    <div style='background-color: #f1f1f1; padding: 10px; text-align: center; font-size: 12px; color: #666;'>
                        <p>&copy; {DateTime.Now.Year} TechXpress. All rights reserved.</p>
                    </div>
                </div>";

        await SendEmailAsync(to, subject, htmlMessage);
    }
}
