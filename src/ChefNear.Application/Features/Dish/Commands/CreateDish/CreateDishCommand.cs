using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public class CreateDishCommand : IRequest<Result<Guid>>
{
    public Guid ChefId { get; set; }
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int QuantityAvailable { get; set; }

    public string? AllergenInfo { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public List<IngredientDto> Ingredients { get; set; } = new();
}

