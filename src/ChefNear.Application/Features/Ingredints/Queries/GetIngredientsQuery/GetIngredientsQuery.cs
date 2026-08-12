using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Ingredints.Queries.GetIngredientsQuery;

public record GetIngredientsQuery(Guid DishId) : IRequest<Result<List<IngredientDtos>>>;
