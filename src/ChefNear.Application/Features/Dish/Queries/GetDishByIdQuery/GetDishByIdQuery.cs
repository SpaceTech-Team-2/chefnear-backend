using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery;

public record GetDishByIdQuery(Guid DishId) : IRequest<Result<DishDetailDto?>>;
