using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace ChefNear.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJWTService jwtService,
            IRefreshTokenService refreshTokenService,
            ILogger<LoginCommandHandler> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _logger = logger;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found with email {Email}", request.Email);
                return DomainErrors.Auth.InvalidCredentials;
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Login failed: Email not confirmed for user {Email}", request.Email);
                return DomainErrors.Auth.EmailNotConfirmed;
            }

            if (user.Status != UserStatus.Active)
            {
                _logger.LogWarning("Login failed: User {Email} is not active (Status: {Status})", request.Email, user.Status);
                return DomainErrors.Auth.AccountNotActive;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed: Invalid password for user {Email}", request.Email);
                return DomainErrors.Auth.InvalidCredentials;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? user.Role.ToString();
            var token = await _jwtService.CreateJwtToken(user, roles);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user);

            _logger.LogInformation("User {Email} logged in successfully with role {Role}", request.Email, roleName);

            return Result.Success(new LoginResponse
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                PhotoUrl = user.PhotoUrl,
                Role = roleName,
                Roles = roles.ToList(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenExpiration = token.ValidTo,
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7),
                TokenType = "Bearer",
                OnboardingCompleted = true,
                CurrentStep = 0
            });
        }
    }
}
