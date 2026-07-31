using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(
            UserManager<User> userManager,
            IRefreshTokenService refreshTokenService,
            ILogger<LogoutCommandHandler> logger)
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _logger = logger;
        }

        public async Task<Result<LogoutResponse>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("Logout failed: User not found with ID {UserId}", request.UserId);
                return DomainErrors.Auth.UserNotFound;
            }

            if (!string.IsNullOrEmpty(request.RefreshToken))
            {
                await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);
                _logger.LogInformation("Refresh token revoked for user {Email}", user.Email);
            }

            _logger.LogInformation("User {Email} logged out successfully", user.Email);

            return Result.Success(new LogoutResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });
        }
    }
}
