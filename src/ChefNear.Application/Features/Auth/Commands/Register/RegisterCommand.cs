using ChefNear.Application.Features.Auth.DTOs;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string DisplayName,
    string Description,
    UserRole Role,
    AddressDto? Address
) : IRequest<Result<RegisterResponse>>;