using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Reviews.DTOs;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Reviews.Queries.GetChefRating
{
    public class GetChefRatingQueryHandler : IRequestHandler<GetChefRatingQuery, Result<ChefRatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetChefRatingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<ChefRatingDto>> Handle(GetChefRatingQuery request, CancellationToken cancellationToken)
        {
            var chef = await _unitOfWork.Users.GetByIdAsync(request.ChefId);
            if (chef == null)
            {
                return Result.Failure<ChefRatingDto>(
                    Error.NotFound("Chef.NotFound", "Chef not found."));
            }
            var reviews = await _unitOfWork.Reviews
            .FindAsync(r => r.Dish.ChefId == request.ChefId);
            if (!reviews.Any())
            {
                return Result.Success(new ChefRatingDto
                {
                    ChefId = request.ChefId,
                    AverageRating = 0,
                    TotalReviews = 0
                });
            }
            var dto = new ChefRatingDto
            {
                ChefId = request.ChefId,
                AverageRating = Math.Round(reviews.Average(r => r.Rating), 1),
                TotalReviews = reviews.Count
            };

            return Result.Success(dto);
        }
    }
}
