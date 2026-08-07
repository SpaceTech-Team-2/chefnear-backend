using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Commands;

public class UpdateAddressCommand : IRequest<Result>
{
    public Guid AddressId { get; set; }

    public Guid UserId { get; set; }

    public string? Label { get; set; }

    public string City { get; set; } = string.Empty;

    public string? Details { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public bool IsDefault { get; set; }
}