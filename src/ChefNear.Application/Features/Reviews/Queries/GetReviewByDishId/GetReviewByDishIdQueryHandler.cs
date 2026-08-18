using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.Responses;
using ChefNear.Shared.Responses.ChefNear.Shared.Responses;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewByDishId
{
    public class GetReviewByDishIdQueryHandler
        : IRequestHandler<
            GetReviewByDishIdQuery,
            Result<PagedResult<ReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetReviewByDishIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PagedResult<ReviewDto>>> Handle(
            GetReviewByDishIdQuery request,
            CancellationToken cancellationToken)
        {
            var query = unitOfWork.Reviews
                .GetQueryable()
                .Where(x => x.DishId == request.DishId);

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items = reviews.Select(review => new ReviewDto
            {
                Id = review.Id,
                OrderId = review.OrderId,
                DishId = review.DishId,
                ClientId = review.ClientId,
                Rating = review.Rating,
                Comment = review.Comment
            }).ToList();

            var result = new PagedResult<ReviewDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Result.Success(result);
        }
    }
}