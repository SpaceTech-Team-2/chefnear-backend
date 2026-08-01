
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Address.DTOs;
using MediatR;

namespace ChefNear.Application.Features.Addresses.Queries;

public class GetUserAddressesQueryHandler
    : IRequestHandler<GetUserAddressesQuery, List<AddressDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserAddressesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<AddressDto>> Handle(
        GetUserAddressesQuery request,
        CancellationToken cancellationToken)
    {
        var addresses = await _unitOfWork.adresses.GetAllAsync();

        return addresses
            .Where(a => a.UserId == request.UserId.ToString())
            .OrderByDescending(a => a.IsDefault)
            .Select(a => new AddressDto
            {
                Id = a.Id,
                Label = a.Label,
                City = a.City,
                Details = a.Details,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                IsDefault = a.IsDefault
            })
            .ToList();
    }
}