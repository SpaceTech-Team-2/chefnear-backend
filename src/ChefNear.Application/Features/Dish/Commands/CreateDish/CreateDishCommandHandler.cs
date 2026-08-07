using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Domain.Enums;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDishCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        var chef = await _unitOfWork.Users.GetByIdAsync(request.ChefId.ToString());

        if (chef == null || chef.Role != UserRole.Chef)
        {
            return Result.Failure<Guid>(
                Error.NotFound("Chef.NotFound", "Chef not found.")
            );
        }

        var dish = new Domain.Entities.Dish
        {
            ChefId = request.ChefId.ToString(),
            CategoryId = request.CategoryId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            QuantityAvailable = request.QuantityAvailable,
            AllergenInfo = request.AllergenInfo,
            Status = DishStatus.Available
        };

        for (int i = 0; i < request.ImageUrls.Count; i++)
        {
            dish.Images.Add(new Domain.Entities.DishImage
            {
                ImageUrl = request.ImageUrls[i],
                IsPrimary = i == 0,
                DisplayOrder = i
            });
        }

        foreach (var item in request.Ingredients)
        {
            dish.Ingredients.Add(new Ingredient
            {
                Name = item.Name,
                Quantity = item.Quantity
            });
        }

        await _unitOfWork.Dishes.AddAsync(dish);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(dish.Id);
    }
}