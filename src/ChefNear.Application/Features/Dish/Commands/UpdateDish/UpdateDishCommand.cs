using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace ChefNear.Application.Features.Dishes.Commands;

public class UpdateDishCommand : IRequest<Result>
{
    public Guid DishId { get; set; }

    [JsonIgnore]

    public Guid ChefId { get; set; } 

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int QuantityAvailable { get; set; }

    public string? AllergenInfo { get; set; }

    public DishStatus Status { get; set; }
}