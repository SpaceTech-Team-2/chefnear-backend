using ChefNear.Application.Features.Auth.Commands.RefreshToken;
using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChefNear.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IJWTService _jwtService;
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(
            IJWTService jwtService,
            UserManager<User> userManager,
            IRefreshTokenService refreshTokenService,
            IOptions<JwtSettings> jwtSettings,
            ILogger<RefreshTokenCommandHandler> logger)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _refreshTokenService.ValidateRefreshTokenAsync(request.RefreshToken);

                if (user == null)
                {
                    _logger.LogWarning("Refresh token validation failed: Invalid or expired token");
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid or expired refresh token"
                    };
                }

                await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);

                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? user.Role.ToString();

                var jwtToken = await _jwtService.CreateJwtToken(user, roles);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user);

                _logger.LogInformation($"Refresh token generated successfully for user {user.Email}");

                return new AuthResponse
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    DisplayName = user.DisplayName,
                    Role = roleName,
                    Roles = roles.ToList(),
                    AccessToken = accessToken,
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    TokenExpiration = jwtToken.ValidTo,
                    RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays > 0
                        ? _jwtSettings.RefreshTokenDurationInDays
                        : 7),
                    TokenType = "Bearer",
                    OnboardingCompleted = true,
                    CurrentStep = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while refreshing token");
                return new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred while refreshing token"
                };
            }
        }
    }
}