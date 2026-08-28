using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Admin.Command.AdminDeleteReview
{
    public class AdminDeleteReviewCommandHandler : IRequestHandler<AdminDeleteReviewCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork;

        public AdminDeleteReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<Result<bool>> Handle(AdminDeleteReviewCommand request, CancellationToken ct)
        {
            var review = await unitOfWork.Reviews
                .GetQueryable()
                .FirstOrDefaultAsync(r => r.Id == request.ReviewId, ct); if (review == null)
                return Result.Failure<bool>(Error.Failure("Review.NotFound", "Review not found."));

               await unitOfWork.Reviews.DeleteAsync(review);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<bool>.Success(true);
        }
    }
}
