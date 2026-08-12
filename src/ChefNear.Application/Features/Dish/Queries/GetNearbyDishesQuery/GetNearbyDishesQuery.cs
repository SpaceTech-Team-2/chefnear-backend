using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Queries.GetNearbyDishesQuery;

public record GetNearbyDishesQuery(
    string? Search = null,
    Guid? CategoryId = null,
    decimal? MaxPrice = null,
    double? ClientLatitude = null,
    double? ClientLongitude = null,
    double? MaxDistanceKm = null,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<List<DishSummaryDto>>>;