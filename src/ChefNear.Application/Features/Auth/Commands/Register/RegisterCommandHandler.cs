using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using Hangfire;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            ILogger<RegisterCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IBackgroundJobClient backgroundJobClient)
        {
            _userManager = userManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                return DomainErrors.Auth.PasswordMismatch;

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return DomainErrors.Auth.EmailAlreadyExists;

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
                _logger.LogWarning("Registration failed for {Email}: {Errors}", request.Email, errors);
                return Error.Failure("Auth.RegistrationFailed", errors);
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

            // Enqueue confirmation email as a background job
            try
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _backgroundJobClient.Enqueue<IEmailService>(
                    svc => svc.SendConfirmationEmailAsync(user.Email ?? "", user.Id, token));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to enqueue confirmation email for {Email}", request.Email);
            }

            _logger.LogInformation("User {Email} registered successfully with role {Role}", request.Email, roleName);

            return Result.Success(new RegisterResponse
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber,
                Role = roleName,
                Roles = new List<string> { roleName },
            });
        }
    }
}