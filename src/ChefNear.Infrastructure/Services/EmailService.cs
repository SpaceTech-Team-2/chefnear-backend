using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ChefNear.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly AppUrlSettings _urlSettings;
        private readonly string _templateBasePath;

        public EmailService(
            IOptions<EmailSettings> options,
            IOptions<AppUrlSettings> urlOptions,
            IOptions<EmailTemplateSettings> templateOptions)
        {
            _settings = options.Value;
            _urlSettings = urlOptions.Value;
            _templateBasePath = templateOptions.Value.TemplatePath;
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
            var encodedUserId = Uri.EscapeDataString(userId);
            var encodedToken = Uri.EscapeDataString(token);
            var confirmUrl = $"{_urlSettings.ApiBaseUrl}/{_urlSettings.ConfirmEmailPath}?userId={encodedUserId}&token={encodedToken}";

            var template = await LoadTemplateAsync("ConfirmEmail.html");
            var body = template
                .Replace("{{UserName}}", to.Split('@')[0])
                .Replace("{{ConfirmUrl}}", confirmUrl)
                .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

            await SendEmailAsync(to, "Confirm your email - ChefNear", body);
        }

        public async Task SendResetPasswordEmailAsync(string to, string resetLink)
        {
            var template = await LoadTemplateAsync("ResetPassword.html");
            var body = template
                .Replace("{{UserName}}", to.Split('@')[0])
                .Replace("{{ResetLink}}", resetLink)
                .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

            await SendEmailAsync(to, "Reset Password - ChefNear", body);
        }

        private async Task<string> LoadTemplateAsync(string templateName)
        {
            var filePath = Path.Combine(_templateBasePath, templateName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Email template '{templateName}' not found at '{filePath}'.");

            return await File.ReadAllTextAsync(filePath);
        }
    }
}
