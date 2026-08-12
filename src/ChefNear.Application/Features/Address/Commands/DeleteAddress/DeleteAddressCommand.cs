using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Commands;

public record DeleteAddressCommand(
    Guid AddressId,
    Guid UserId
) : IRequest<Result>;