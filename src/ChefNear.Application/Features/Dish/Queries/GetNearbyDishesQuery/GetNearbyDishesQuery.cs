using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Queries.GetNearbyDishesQuery;

public class GetNearbyDishesQuery : IRequest<Result<List<DishSummaryDto>>>
{
    public string? Search { get; set; }

    public Guid? CategoryId { get; set; }

    public decimal? MaxPrice { get; set; }

    public double? ClientLatitude { get; set; }

    public double? ClientLongitude { get; set; }

    public double? MaxDistanceKm { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}