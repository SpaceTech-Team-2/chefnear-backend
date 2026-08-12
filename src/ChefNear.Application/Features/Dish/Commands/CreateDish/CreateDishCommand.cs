using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public record CreateDishRequest(
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    int QuantityAvailable,
    string? AllergenInfo,
    List<string> ImageUrls,
    List<IngredientDto> Ingredients
);

public record CreateDishCommand(
    Guid ChefId,
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    int QuantityAvailable,
    string? AllergenInfo,
    List<string> ImageUrls,
    List<IngredientDto> Ingredients
) : IRequest<Result<Guid>>;
