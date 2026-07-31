using ChefNear.Application.Interfaces;
using ChefNear.Application.Model;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace ChefNear.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
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

        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _refreshTokenService.ValidateRefreshTokenAsync(request.RefreshToken);

                if (user == null)
                {
                    _logger.LogWarning("Refresh token validation failed: Invalid or expired token");
                    return DomainErrors.Auth.InvalidToken;
                }

                await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken);

                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? user.Role.ToString();

                var jwtToken = await _jwtService.CreateJwtToken(user, roles);
                var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user);

                _logger.LogInformation("Refresh token generated successfully for user {Email}", user.Email);

                return Result.Success(new RefreshTokenResponse
                {
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    DisplayName = user.DisplayName,
                    Role = roleName,
                    Roles = roles.ToList(),
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenExpiration = jwtToken.ValidTo,
                    RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays > 0
                        ? _jwtSettings.RefreshTokenDurationInDays
                        : 7),
                    TokenType = "Bearer",
                    OnboardingCompleted = true,
                    CurrentStep = 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while refreshing token");
                return DomainErrors.Auth.RefreshTokenFailed;
            }
        }
    }
}