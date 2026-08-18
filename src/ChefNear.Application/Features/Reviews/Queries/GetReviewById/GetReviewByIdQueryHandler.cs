using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler
        : IRequestHandler<GetReviewByIdQuery, Result<ReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetReviewByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<ReviewDto>> Handle(
            GetReviewByIdQuery request,
            CancellationToken cancellationToken)
        {
            var review = await unitOfWork.Reviews
                .GetByIdAsync(request.ReviewId);

            if (review == null)
            {
                return Result.Failure<ReviewDto>(
                    new Error("Review.NotFound", "Review not found."));
            }

            var reviewDto = new ReviewDto
            {
                Id = review.Id,
                OrderId = review.OrderId,
                DishId = review.DishId,
                ClientId = review.ClientId,
                Rating = review.Rating,
                Comment = review.Comment
            };

            return Result.Success(reviewDto);
        }
    }
}