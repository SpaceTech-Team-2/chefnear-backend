using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Ingredints.Commands.RemoveIngredient
{
    public class RemoveIngredientCommandHandler
        : IRequestHandler<RemoveIngredientCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveIngredientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _unitOfWork.ingredients.GetByIdAsync(request.IngredientId);

            if (ingredient == null)
                return Result<Guid>.Failure( Error.NotFound("Ingredient.NotFound", "Ingredient not found."));

            var dish = await _unitOfWork.dishes.GetByIdAsync(ingredient.DishId);

            if (dish == null)
                return Result<Guid>.Failure(Error.NotFound("Dish.NotFound", "Dish not found."));

            if (dish.ChefId != request.ChefId)
                return Result<Guid>.Failure(Error.Forbidden("Dish.NotOwner", "Only the owner can remove this ingredient."));

           await _unitOfWork.ingredients.DeleteAsync(ingredient);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}