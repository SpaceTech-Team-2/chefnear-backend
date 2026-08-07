using ChefNear.Application.Features.Address.DTOs;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Queries;

public class GetUserAddressesQuery : IRequest<List<AddressDto>>
{
    public Guid UserId { get; set; }
}