using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Address.Queries.GetAddressById;

public record GetAddressByIdQuery(Guid AddressId) : IRequest<Result<AddressDto>>;
