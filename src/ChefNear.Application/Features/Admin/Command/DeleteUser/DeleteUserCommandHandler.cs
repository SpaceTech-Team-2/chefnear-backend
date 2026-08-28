using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Command.DeleteUser
{
  

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                  return Result.Failure<bool>(
                    Error.Failure(
                        "Admin.UserNotFound",
                        "User not found."));

            var identityResult = await _userManager.DeleteAsync(user);
            if (!identityResult.Succeeded)
                return Result.Failure<bool>(
    Error.Failure(
        "Admin.DeletedFailed",
        string.Join(", ", identityResult.Errors.Select(e => e.Description))));

            return Result<bool>.Success(true);
        }
    }
}
