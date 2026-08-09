using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Queries.GetNearbyDishesQuery;

public class GetNearbyDishesQueryHandler
    : IRequestHandler<
        GetNearbyDishesQuery,
        Result<List<DishSummaryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetNearbyDishesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<DishSummaryDto>>> Handle(
        GetNearbyDishesQuery request,
        CancellationToken cancellationToken)
    {
        var dishes = await _unitOfWork.Dishes.GetNearbyDishesAsync();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            dishes = dishes
                .Where(d =>
                    d.Name.Contains(
                        request.Search,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (request.CategoryId.HasValue)
        {
            dishes = dishes
                .Where(d => d.CategoryId == request.CategoryId.Value)
                .ToList();
        }

        if (request.MaxPrice.HasValue)
        {
            dishes = dishes
                .Where(d => d.Price <= request.MaxPrice.Value)
                .ToList();
        }

        var result = dishes
            .Select(d => new DishSummaryDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                PrimaryImageUrl =
                    d.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl,
                ChefDisplayName =
                    d.Chef.DisplayName ?? "Chef",
                DistanceKm = 0
            })
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Result.Success(result);
    }
}