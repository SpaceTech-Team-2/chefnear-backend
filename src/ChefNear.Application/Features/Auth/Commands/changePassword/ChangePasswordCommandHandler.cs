using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Auth.Commands.changePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordComand, Result<ChangePasswordResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

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
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ChangePasswordResponse>> Handle(ChangePasswordComand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return DomainErrors.Auth.UserNotAuthenticated;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Change password failed: User not found with ID {UserId}", userId);
                return DomainErrors.Auth.UserNotFound;
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OLdPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Change password failed for user {Email}: {Errors}", user.Email, errors);
                return Error.Failure("Auth.ChangePasswordFailed", errors);
            }

            await _unitOfWork.SaveChangesAsync();
            await _refreshTokenService.RevokeRefreshTokenAsync(user.Id);

            _logger.LogInformation("Password changed successfully for user {Email}", user.Email);

            return Result.Success(new ChangePasswordResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName
            });
        }
    }
}