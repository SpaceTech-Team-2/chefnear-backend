using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Shared.ResultPattern;
using ChefNear.Domain.Entities;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ChefNear.Application.Features.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordComand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly AppUrlSettings _appUrlSettings;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ForgetPasswordCommandHandler(
            UserManager<User> userManager,
            IOptions<AppUrlSettings> appUrlSettings,
            IBackgroundJobClient backgroundJobClient)
        {
            _userManager = userManager;
            _appUrlSettings = appUrlSettings.Value;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<Result> Handle(ForgetPasswordComand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Return success even if user not found to prevent email enumeration
                return Result.Success();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var encodedEmail = Uri.EscapeDataString(user.Email!);

            var resetLink = $"{_appUrlSettings.FrontendBaseUrl}/{_appUrlSettings.ResetPasswordPath}?email={encodedEmail}&token={encodedToken}";

            // Enqueue reset password email as a background job
            _backgroundJobClient.Enqueue<IEmailService>(
                svc => svc.SendResetPasswordEmailAsync(user.Email!, resetLink));

            return Result.Success();
        }
    }
}