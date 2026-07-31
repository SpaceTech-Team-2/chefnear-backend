using ChefNear.Application.Interfaces;
using ChefNear.Application.Responce;
using ChefNear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, BaseCommandResponse>
    {
        private readonly UserManager<User> userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRefreshTokenService refreshTokenService;

        public ResetPasswordCommandHandler(UserManager<User> userManager, ICurrentUserService currentUserService, IRefreshTokenService refreshTokenService)
        {
            this.userManager = userManager;
            _currentUserService = currentUserService;
            this.refreshTokenService = refreshTokenService;
        }

        public async Task<BaseCommandResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new BaseCommandResponse
                {
                    Success = true,
                    Message = "If an account with this email exists, a reset link has been sent."
                };
            }
            if (request.NewPassword != request.ConfirmPassword)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Passwords do not match."
                };
            }
            var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(x => x.Description))
                };
            }
            await refreshTokenService.RevokeRefreshTokenAsync(user.Id.ToString());
            return new BaseCommandResponse
            {
                Success = true,
                Message = "Password reset successfully."
            };
        }
    }
}
