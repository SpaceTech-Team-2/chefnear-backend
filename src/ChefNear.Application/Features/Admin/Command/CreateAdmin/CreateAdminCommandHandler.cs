using ChefNear.Domain.Entities;
using ChefNear.Shared.Constants;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Command.CreateAdmin
{
    public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public CreateAdminCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(CreateAdminCommand request, CancellationToken ct)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
                return Result.Failure<string>(
                    Error.Failure(
                        "Admin.EmailAlreadyExists",
                        "Email already exists."));
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FullName,
                LastName = string.Empty,
                EmailConfirmed = true
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
                return Result.Failure<string>(
    Error.Failure(
        "Admin.CreationFailed",
        string.Join(", ", identityResult.Errors.Select(e => e.Description))));
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Result<string>.Success(user.Id);
        }
    }
}
