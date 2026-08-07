using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Address.Queries.GetAddressById
{
    public class GetAddresByIdHandler : IRequestHandler<GetAddressByIdQuery, Result<AddressDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAddresByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<AddressDto>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.adresses.GetByIdAsync(request.AddressId);

            if (address == null)
            {
                return Result.Failure<AddressDto>(
                    Error.NotFound("Address.NotFound", "Address not found."));
            }

            var dto = new AddressDto
            {
                Id = address.Id,
                Label = address.Label,
                City = address.City,
                Details = address.Details,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                IsDefault = address.IsDefault
            };

            return Result.Success(dto);
        }
    }
}
 