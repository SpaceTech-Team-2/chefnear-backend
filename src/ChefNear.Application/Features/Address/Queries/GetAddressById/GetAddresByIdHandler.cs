using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Address.Queries.GetAddressById
{
    public class GetAddresByIdHandler : IRequestHandler<GetAddressByIdQuery, Result<GetAddressDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAddresByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAddressDto>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.Adresses.GetByIdAsync(request.AddressId);

            if (address == null)
            {
                return Result.Failure<GetAddressDto>(
                    Error.NotFound("Address.NotFound", "Address not found."));
            }

            var dto = new GetAddressDto
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
 