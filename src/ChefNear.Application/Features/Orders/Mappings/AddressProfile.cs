using AutoMapper;
using ChefNear.Application.Features.Auth.DTOs;
namespace ChefNear.Application.Features.Orders.Mappings;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<CreateAddressDto, Domain.Entities.Address>();
    }

}
