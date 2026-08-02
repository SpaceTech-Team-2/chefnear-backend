using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Dish.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery
{
    public class GetDishByIdQueryHandler
      : IRequestHandler<GetDishByIdQuery, DishDetailDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDishByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DishDetailDto?> Handle(
            GetDishByIdQuery request,
            CancellationToken cancellationToken)
        {
            var dish = await _unitOfWork.dishes.GetByIdAsync(request.DishId);

            if (dish == null || dish.IsDeleted)
                return null;

            return new DishDetailDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                QuantityAvailable = dish.QuantityAvailable,
                AllergenInfo = dish.AllergenInfo,
                CategoryName = dish.Category.Name,
                ChefId = dish.ChefId,
                ChefDisplayName = dish.Chef.DisplayName ?? "Chef",
                ChefReliabilityScore = dish.Chef.ReliabilityScore ?? 5.0,

                Images = dish.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new DishImageDto
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        IsPrimary = i.IsPrimary,
                        DisplayOrder = i.DisplayOrder
                    })
                    .ToList(),

                Ingredients = dish.Ingredients
                    .Select(i => new IngredientDtos
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Quantity = i.Quantity
                    })
                    .ToList()
            };
        }
    }
}
