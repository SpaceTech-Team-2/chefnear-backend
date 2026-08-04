using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Address.Queries.GetAddressById
{
    public class GetAddressByIdQuery : IRequest<Result<AddressDto>> 
    {
        public Guid AddressId { get; set; }

    }
}
