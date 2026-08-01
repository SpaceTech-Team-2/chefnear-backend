using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Commands;

public class DeleteAddressCommand : IRequest<Result>
{
    public Guid AddressId { get; set; }

    public Guid UserId { get; set; }
}