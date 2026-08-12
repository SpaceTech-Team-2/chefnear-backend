using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public record UpdateDishRequest(
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    int QuantityAvailable,
    string? AllergenInfo,
    DishStatus Status
);

public record UpdateDishCommand(
    Guid DishId,
    Guid ChefId,
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    int QuantityAvailable,
    string? AllergenInfo,
    DishStatus Status
) : IRequest<Result>;