using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Commands;

public record UpdateAddressRequest(
    string? Label,
    string City,
    string? Details,
    double Latitude,
    double Longitude,
    bool IsDefault
);

public record UpdateAddressCommand(
    Guid AddressId,
    Guid UserId,
    string? Label,
    string City,
    string? Details,
    double Latitude,
    double Longitude,
    bool IsDefault
) : IRequest<Result>;