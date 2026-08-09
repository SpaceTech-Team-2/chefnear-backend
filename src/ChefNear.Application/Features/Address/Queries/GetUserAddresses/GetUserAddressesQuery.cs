using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Queries;

public class GetUserAddressesQuery : IRequest<Result<List<AddressDto>>>
{
    public Guid UserId { get; set; }
}