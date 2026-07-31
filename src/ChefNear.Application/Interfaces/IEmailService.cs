using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendConfirmationEmailAsync(string to, string userId, string token);




    }
}
