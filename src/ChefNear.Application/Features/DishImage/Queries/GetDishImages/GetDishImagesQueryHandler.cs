using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.DishImage.Queries.GetDishImages
{
   

        public class GetDishImagesQueryHandler
            : IRequestHandler<GetDishImagesQuery, Result<List<DishImageDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDishImagesQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<List<DishImageDto>>> Handle(
                GetDishImagesQuery request,
                CancellationToken cancellationToken)
            {
                var dish = await _unitOfWork.dishes.GetByIdAsync(request.DishId);

                if (dish == null || dish.IsDeleted)
                {
                return Result.Failure<List<DishImageDto>>(
  Error.NotFound("Dish.NotFound", "Dish not found."));
            }

                var images = await _unitOfWork.dishImages.GetAllAsync();

                var result = images
                    .Where(i => i.DishId == request.DishId)
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new DishImageDto
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        IsPrimary = i.IsPrimary,
                        DisplayOrder = i.DisplayOrder
                    })
                    .ToList();

                return Result.Success(result);
            }
        }
}

