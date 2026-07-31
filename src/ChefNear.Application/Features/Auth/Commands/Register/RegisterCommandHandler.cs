using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChefNear.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            IEmailService emailService,
            ILogger<RegisterCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                return new AuthResponse { Success = false, Message = "Passwords do not match" };

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return new AuthResponse { Success = false, Message = "Email already exists" };

            // ✅ 1. اعمل الـ User الأول من غير KitchenAddressId (سيبه null مؤقتاً)
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName ?? request.Email.Split('@')[0],
                PhoneNumber = request.PhoneNumber,
                PhotoUrl = request.PhotoUrl,
                Description = request.Description,
                Role = request.Role,
                Status = UserStatus.Active,
                KitchenAddressId = null   
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning($"Registration failed for {request.Email}: {errors}");
                return new AuthResponse { Success = false, Message = errors };
            }

            var roleName = request.Role.ToString();
            if (!await _userManager.IsInRoleAsync(user, roleName))
                await _userManager.AddToRoleAsync(user, roleName);

            if (request.Address != null)
            {
                var address = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,              
                    Label = request.Address.Label,
                    City = request.Address.City,
                    Details = request.Address.Details,
                    Latitude = request.Address.Latitude,
                    Longitude = request.Address.Longitude,
                    IsDefault = request.Address.IsDefault,
                    CreatedAt = DateTime.UtcNow,
                };

                await _unitOfWork.adresses.AddAsync(address);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (request.Role == UserRole.Chef)
                {
                    user.KitchenAddressId = address.Id;
                    await _userManager.UpdateAsync(user);   
                }
            }

            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailService.SendConfirmationEmailAsync(user.Email ?? "", user.Id, token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to send confirmation email to {request.Email}");
            }

            _logger.LogInformation($"User {request.Email} registered successfully with role {roleName}");

            return new AuthResponse
            {
                Success = true,
                Message = "Registration successful. Please check your email to confirm your account.",
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                Role = roleName,
                Roles = new List<string> { roleName },
                OnboardingCompleted = false,
            };
        }
    }
}