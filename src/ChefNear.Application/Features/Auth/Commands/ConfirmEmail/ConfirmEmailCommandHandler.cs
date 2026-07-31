using ChefNear.Application.Interfaces;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ChefNear.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, BaseCommandResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(
            UserManager<User> userManager,
            IEmailService emailService,
            ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<BaseCommandResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning($"Confirm email failed: User not found with ID {request.UserId}");
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (user.EmailConfirmed)
            {
                return new BaseCommandResponse
                {
                    Success = true,
                    Message = "Email already confirmed"
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"Confirm email failed for user {user.Email}: Invalid or expired token");
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Invalid or expired confirmation link."
                };
            }

            _logger.LogInformation($"Email confirmed successfully for user {user.Email}");

            return new BaseCommandResponse
            {
                Success = true,
                Message = "Email confirmed successfully.",
                Data = new
                {
                    UserId = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName
                }
            };
        }
    }
}