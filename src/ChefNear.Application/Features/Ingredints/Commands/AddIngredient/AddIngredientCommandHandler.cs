using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Ingredints.Commands.AddIngredient;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Application.Features.Ingredints.AddIngredient
{
  

    public class AddIngredientCommandHandler
        : IRequestHandler<AddIngredientCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddIngredientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddIngredientCommand request, CancellationToken cancellationToken)
        {
            var dish = await _unitOfWork.dishes.GetByIdAsync(request.DishId);

            if (dish == null || dish.IsDeleted)
            {
                return Error.NotFound("Dish.NotFound", "Dish not found.");
            }

            if (dish.ChefId != request.ChefId)
            {
                return Error.Forbidden("Dish.NotOwner", "Only the owner can add ingredients.");
            }

            var ingredient = new Ingredient
            {
                DishId = request.DishId,
                Name = request.Name,
                Quantity = request.Quantity
            };

            await _unitOfWork.ingredients.AddAsync(ingredient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(ingredient.Id);
        }
    }
}
