using AutoMapper;
using ChefNear.Application.Features.Auth.DTOs;
using ChefNear.Domain.Entities;

namespace ChefNear.Application.Features.Orders.Mappings;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<AddressDto, Address>();
    }

}
