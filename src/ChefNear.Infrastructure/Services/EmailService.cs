using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace ChefNear.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly AppUrlSettings _urlSettings;  

        public EmailService(IOptions<EmailSettings> options, IOptions<AppUrlSettings> urlOptions)
        {
            _settings = options.Value;
            _urlSettings = urlOptions.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(_settings.DisplayName, _settings.Email));

                email.To.Add(MailboxAddress.Parse(to));

                email.Subject = subject;
                email.Body = new TextPart("html")
                {
                    Text = body
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.Email, _settings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send email to {to}: {ex.Message}");
            }
        }
        public async Task SendConfirmationEmailAsync(string to, string userId, string token)
        {
            var subject = "Confirm your email - ChefNear";

            var encodedUserId = Uri.EscapeDataString(userId);
            var encodedToken = Uri.EscapeDataString(token);

            var confirmUrl = $"{_urlSettings.ApiBaseUrl}/{_urlSettings.ConfirmEmailPath}?userId={encodedUserId}&token={encodedToken}";

            var body = $@"
    <html>
        <body style='font-family: Arial, sans-serif; direction: ltr;'>
            <h2>Welcome to ChefNear! 🍳</h2>
            <p>Please confirm your email by clicking the link below:</p>
            <a href='{confirmUrl}'>
                Confirm Email
            </a>
            <p>If you didn't create this account, please ignore this email.</p>
            <hr/>
            <p>Best regards,<br/>ChefNear Team</p>
        </body>
    </html>";
            await SendEmailAsync(to, subject, body);
        }
    }
}
