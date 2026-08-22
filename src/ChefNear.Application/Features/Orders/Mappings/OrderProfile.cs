using AutoMapper;
using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Application.Features.Orders.DTOs;
using ChefNear.Domain.Entities;

namespace ChefNear.Application.Features.Orders.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dto => dto.DishName, opt => opt.MapFrom(src => src.Dish.Name))
            .ForMember(dto => dto.SubTotal, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));

        CreateMap<Order, GetOrderByIdDto>()
            .ForMember(dto => dto.ChefName, opt => opt.MapFrom(src => src.Chef.DisplayName))
            .ForMember(dto => dto.ClientName, opt => opt.MapFrom(src => src.Client.DisplayName))
            .ForMember(dto => dto.Items, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dto => dto.Address, opt => opt.MapFrom(src => src.DeliveryAddress));

        CreateMap<Domain.Entities.Address, GetAddressDto>();
        CreateMap<Order, ChefOrderDto>()
            .ForMember(dto => dto.Items, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dto => dto.Address, opt => opt.MapFrom(src => src.DeliveryAddress));

    }
}
