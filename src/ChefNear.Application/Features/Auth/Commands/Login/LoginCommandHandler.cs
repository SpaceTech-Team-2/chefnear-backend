using ChefNear.Application.Interfaces;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
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

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"Login failed: User not found with email {request.Email}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning($"Login failed: Email not confirmed for user {request.Email}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Please confirm your email before logging in."
                };
            }

            if (user.Status != UserStatus.Active)
            {
                _logger.LogWarning($"Login failed: User {request.Email} is not active (Status: {user.Status})");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Your account is not active. Please contact support."
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"Login failed: Invalid password for user {request.Email}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? user.Role.ToString();
            var token = await _jwtService.CreateJwtToken(user, roles);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user);

            _logger.LogInformation($"User {request.Email} logged in successfully with role {roleName}");
            return new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                PhotoUrl = user.PhotoUrl,
                Role = roleName,
                Roles = roles.ToList(),
                AccessToken = accessToken,
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiration = token.ValidTo,
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(7),
                TokenType = "Bearer",
                OnboardingCompleted = true,
                CurrentStep = 0
            };
        }
    }
}
