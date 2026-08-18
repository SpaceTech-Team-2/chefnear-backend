using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Application.Features.Reviews.Queries.getReviewByDishId;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewByDishId
{
    public class GetAverageReviewByDishIdQueryHandler
        : IRequestHandler<GetAverageReviewByDishIdQuery, Result<AverageDishReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAverageReviewByDishIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<AverageDishReviewDto>> Handle(
            GetAverageReviewByDishIdQuery request,
            CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.Reviews
                .GetByDishIdAsync(request.DishId, cancellationToken);

            if (!reviews.Any())
            {
                return Result.Success(new AverageDishReviewDto
                {
                    DishId = request.DishId,
                    TotalReviews = 0,
                    AverageRating = 0
                });
            }

            var result = new AverageDishReviewDto
            {
                DishId = request.DishId,
                TotalReviews = reviews.Count(),
                AverageRating = reviews.Average(x => x.Rating)
            };

            return Result.Success(result);
        }
    }
}