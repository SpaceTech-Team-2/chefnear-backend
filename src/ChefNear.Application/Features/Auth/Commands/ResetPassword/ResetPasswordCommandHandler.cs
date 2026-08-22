using ChefNear.Application.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChefNear.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;

        public ResetPasswordCommandHandler(
            UserManager<User> userManager,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Return success to prevent email enumeration
                return Result.Success();
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return Result.Failure(DomainErrors.Auth.PasswordMismatch);
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                return Result.Failure(Error.Validation("Auth.ResetPasswordFailed", errors));
            }

            await _refreshTokenService.RevokeRefreshTokenAsync(user.Id.ToString());

            return Result.Success();
        }
    }
}
