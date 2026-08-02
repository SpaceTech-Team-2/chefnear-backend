using ChefNear.Application.Features.Auth.DTOs;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ChefNear.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<Result<RegisterResponse>>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string? DisplayName { get; set; }

    public string? PhoneNumber { get; set; }

    public IFormFile? Photo { get; set; }

    public string? Description { get; set; }

    public UserRole Role { get; set; } = UserRole.Client;

    public AddressDto? Address { get; set; }
}