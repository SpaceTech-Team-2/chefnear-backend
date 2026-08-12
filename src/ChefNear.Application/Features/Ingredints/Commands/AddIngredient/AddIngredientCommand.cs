using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Ingredints.Commands.AddIngredient;

public record AddIngredientRequest(
    Guid DishId,
    string Name,
    string? Quantity
);

public record AddIngredientCommand(
    Guid DishId,
    string ChefId,
    string Name,
    string? Quantity
) : IRequest<Result<Guid>>;
