using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public class UpdateDishCommand : IRequest<Result>
{
    public Guid DishId { get; set; }

    public string ChefId { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int QuantityAvailable { get; set; }

    public string? AllergenInfo { get; set; }

    public DishStatus Status { get; set; }
}