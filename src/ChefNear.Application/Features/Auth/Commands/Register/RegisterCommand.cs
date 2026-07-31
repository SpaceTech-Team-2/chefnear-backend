using ChefNear.Application.Features.Auth.DTOs;
using ChefNear.Application.Responce;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

public class RegisterCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Description { get; set; }
    public UserRole Role { get; set; } = UserRole.Client;
    public AddressDto? Address { get; set; }  
}