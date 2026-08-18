using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChefNear.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Result>
    {
        private const int EditWindowDays = 3;

        private readonly IUnitOfWork _unitOfWork;

        public UpdateReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async  Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(request.ReviewId);
         
            if (review == null)
            {
                return Result.Failure(
                    Error.NotFound("Review.NotFound", "Review not found."));
            }

            if (review.ClientId != request.ClientId)
            {
                return Result.Failure(
                    Error.Forbidden("Review.NotOwner", "Only the client who wrote this review can edit it."));
            }
            var daysSinceCreated = (DateTime.UtcNow - review.CreatedAt).TotalDays;
            if (daysSinceCreated > EditWindowDays)
            {
                return Result.Failure(
                    Error.Validation("Review.EditWindowExpired",
                        $"Reviews can only be edited within {EditWindowDays} days of submission."));
            }
            review.Rating = request.Rating;
            review.Comment = request.Comment;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
