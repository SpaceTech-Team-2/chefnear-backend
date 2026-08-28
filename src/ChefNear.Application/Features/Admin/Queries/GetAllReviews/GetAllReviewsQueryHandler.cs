using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Admin.Queries.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Application.Features.Admin.Queries.GetAllReviews
{
    public class GetAllReviewsQueryHandler
        : IRequestHandler<GetAllReviewsQuery, Result<List<AdminReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllReviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<AdminReviewDto>>> Handle(
            GetAllReviewsQuery request,
            CancellationToken ct)
        {
            var reviews = await unitOfWork.Reviews
                .GetQueryable()
                .AsNoTracking()
                .Include(r => r.Client)
                .Include(r => r.Dish)
                    .ThenInclude(d => d.Chef)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(r => new AdminReviewDto
                {
                    Id = r.Id,
                    ClientName = r.Client.FirstName,
                    ChefName = r.Dish.Chef.FirstName,
                    DishId = r.DishId,
                    DishName = r.Dish.Name,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync(ct);

            return Result.Success(reviews);
        }
    }
}