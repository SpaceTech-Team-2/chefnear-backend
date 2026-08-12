using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImage.Queries.GetDishImages;

public record GetDishImagesQuery(Guid DishId) : IRequest<Result<List<DishImageDto>>>;
