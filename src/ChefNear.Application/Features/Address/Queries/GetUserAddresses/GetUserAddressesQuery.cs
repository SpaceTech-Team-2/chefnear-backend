using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Queries;

public record GetUserAddressesQuery(Guid UserId) : IRequest<Result<List<AddressDto>>>;
