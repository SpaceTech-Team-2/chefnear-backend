using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Ingredints.Commands.UpdateIngredient
{
    public class UpdateIngredientCommandHandler
        : IRequestHandler<UpdateIngredientCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateIngredientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _unitOfWork.Ingredients.GetByIdAsync(request.IngredientId);

            if (ingredient == null)
            {
                return Result.Failure(
    Error.NotFound("Ingredient.NotFound", "Ingredient not found."));
            }

            var dish = await _unitOfWork.Dishes.GetByIdAsync(ingredient.DishId);

            if (dish == null)
            {
                return Result.Failure( Error.NotFound(
                    "Dish.NotFound",
                    "Dish not found."));
            }

            if (dish.ChefId != request.ChefId)
            {
                return Result.Failure( Error.Forbidden(
                    "Dish.NotOwner",
                    "Only the owner can update this ingredient."));
            }

            ingredient.Name = request.Name;
            ingredient.Quantity = request.Quantity;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}