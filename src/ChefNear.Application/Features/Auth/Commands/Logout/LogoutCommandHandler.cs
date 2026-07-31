using ChefNear.Application.Interfaces;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, AuthResponse>
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

        public async Task<AuthResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning($"Logout failed: User not found with ID {request.UserId}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!string.IsNullOrEmpty(request.RefreshToken))
            {
                await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);
                _logger.LogInformation($"Refresh token revoked for user {user.Email}");
            }



            _logger.LogInformation($"User {user.Email} logged out successfully");

            return new AuthResponse
            {
                Success = true,
                Message = "Logged out successfully",
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
        }
    
