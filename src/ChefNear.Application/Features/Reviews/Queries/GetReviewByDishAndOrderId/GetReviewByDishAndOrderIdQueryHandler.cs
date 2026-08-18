using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Queries.GetReviewByDishAndOrderId
{
    public class GetReviewByDishAndOrderIdQueryHandler
    : IRequestHandler<
        GetReviewByDishAndOrderIdQuery,
        Result<ReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetReviewByDishAndOrderIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<ReviewDto>> Handle(
            GetReviewByDishAndOrderIdQuery request,
            CancellationToken cancellationToken)
        {
            var review = await unitOfWork.Reviews.GetAsync(
                x => x.DishId == request.DishId &&
                     x.OrderId == request.OrderId);

            if (review == null)
            {
                return Result.Failure<ReviewDto>(
                    new Error(
                        "Review.NotFound",
                        "Review not found.",
                        ErrorType.NotFound));
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
