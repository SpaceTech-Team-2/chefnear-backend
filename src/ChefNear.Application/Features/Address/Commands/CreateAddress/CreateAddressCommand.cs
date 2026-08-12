using ChefNear.Shared.ResultPattern;
using MediatR;

namespace HomeChefMarketplace.Application.Features.Addresses.Commands;

public record CreateAddressRequest(
    string? Label,
    string City,
    string? Details,
    double Latitude,
    double Longitude,
    bool IsDefault
);

public record CreateAddressCommand(
    Guid UserId,
    string? Label,
    string City,
    string? Details,
    double Latitude,
    double Longitude,
    bool IsDefault
) : IRequest<Result<Guid>>;