using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Ingredints.Commands.UpdateIngredient;

public record UpdateIngredientRequest(
    Guid IngredientId,
    string Name,
    string? Quantity
);

public record UpdateIngredientCommand(
    Guid IngredientId,
    string ChefId,
    string Name,
    string? Quantity
) : IRequest<Result>;
