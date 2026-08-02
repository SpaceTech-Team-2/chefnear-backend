using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Ingredints.Queries.GetIngredientsQuery
{
    public class GetIngredientsQueryHandler
        : IRequestHandler<GetIngredientsQuery, Result<List<IngredientDtos>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetIngredientsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<IngredientDtos>>> Handle(
            GetIngredientsQuery request,
            CancellationToken cancellationToken)
        {
            var ingredients = await _unitOfWork.ingredients.GetAllAsync();

            var result = ingredients
                .Where(i => i.DishId == request.DishId)
                .Select(i => new IngredientDtos
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                })
                .ToList();

            return Result.Success(result);
        }
    }
}