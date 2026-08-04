using FluentValidation;
using HomeChefMarketplace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            // Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            // Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            // Confirm Password
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            // Display Name
            RuleFor(x => x.DisplayName)
                .MaximumLength(100).WithMessage("Display name cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.DisplayName));

            // Phone Number
            RuleFor(x => x.PhoneNumber).NotEmpty()
                .Matches(@"^01[0-9]{9}$").WithMessage("Invalid phone number format");

            // Role
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Invalid role");

            // Address (client)
            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address!.City)
                    .NotEmpty().WithMessage("City is required");

                RuleFor(x => x.Address!.Latitude)
                    .InclusiveBetween(-90, 90).WithMessage("Invalid latitude");

                RuleFor(x => x.Address!.Longitude)
                    .InclusiveBetween(-180, 180).WithMessage("Invalid longitude");
            });

           
        }
    }
}
