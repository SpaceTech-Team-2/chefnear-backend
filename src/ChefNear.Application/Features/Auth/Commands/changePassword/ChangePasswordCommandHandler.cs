using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Auth.Commands.changePassword;
using ChefNear.Application.Interfaces;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChefNear.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordComand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;
        private readonly IUnitOfWork unitOfWork;
        public ChangePasswordCommandHandler(
            UserManager<User> userManager,
            ICurrentUserService currentUserService,
            IRefreshTokenService refreshTokenService,
            ILogger<ChangePasswordCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _refreshTokenService = refreshTokenService;
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> Handle(ChangePasswordComand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not authenticated"
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"Change password failed: User not found with ID {userId}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OLdPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning($"Change password failed for user {user.Email}: {errors}");
                return new AuthResponse
                {
                    Success = false,
                    Message = errors
                };
            }
            await unitOfWork.SaveChangesAsync();

             await _refreshTokenService.RevokeRefreshTokenAsync(user.Id);

            _logger.LogInformation($"Password changed successfully for user {user.Email}");

            return new AuthResponse
            {
                Success = true,
                Message = "Password changed successfully",
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName
            };
        }
    }
}