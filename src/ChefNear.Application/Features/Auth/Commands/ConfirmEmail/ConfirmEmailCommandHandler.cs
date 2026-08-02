using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(
            UserManager<User> userManager,
            ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("Confirm email failed: User not found with ID {UserId}", request.UserId);
                return Result.Failure(DomainErrors.Auth.UserNotFound);
            }

            if (user.EmailConfirmed)
            {
                return Result.Success();
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Confirm email failed for user {Email}: Invalid or expired token", user.Email);
                return Result.Failure(DomainErrors.Auth.InvalidToken);
            }

            _logger.LogInformation("Email confirmed successfully for user {Email}", user.Email);

            return Result.Success();
        }
    }
}