namespace ChefNear.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendConfirmationEmailAsync(string to, string userId, string token);
        Task SendResetPasswordEmailAsync(string to, string resetLink);
    }
}
