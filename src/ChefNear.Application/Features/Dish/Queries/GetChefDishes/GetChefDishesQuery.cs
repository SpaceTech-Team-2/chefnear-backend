using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dish.Queries.GetChefDishes;

public record GetChefDishesQuery(
    string ChefId
): IRequest<Result<List<DishSummaryDto>>>;