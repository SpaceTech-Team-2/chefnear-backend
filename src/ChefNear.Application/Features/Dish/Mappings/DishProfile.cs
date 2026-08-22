using AutoMapper;
using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Domain.Entities;

namespace ChefNear.Application.Features.Dish.Mappings;

public class DishProfile : Profile
{
    public DishProfile()
    {
        CreateMap<ChefNear.Domain.Entities.Dish, DishSummaryDto>()
            .ForMember(dto => dto.PrimaryImageUrl, opt => opt.MapFrom(src => src.Images.Any(x => x.IsPrimary) ? src.Images.First(i => i.IsPrimary).ImageUrl : ""))
            .ForMember(dto => dto.ChefDisplayName, opt => opt.MapFrom(src => src.Chef.DisplayName))
            .ForMember(dto => dto.DistanceKm, opt => opt.MapFrom(src => 0));
    }
}
