using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDishCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        var dish = await _unitOfWork.dishes.GetByIdAsync(request.DishId);

        if (dish == null || dish.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("Dish.NotFound", "Dish not found.")
            );
        }

        if (dish.ChefId != request.ChefId)
        {
            return Result.Failure(
                Error.Forbidden("Dish.Forbidden", "Only the owning chef can update this dish.")
            );
        }

        dish.CategoryId = request.CategoryId;
        dish.Name = request.Name;
        dish.Description = request.Description;
        dish.Price = request.Price;
        dish.QuantityAvailable = request.QuantityAvailable;
        dish.AllergenInfo = request.AllergenInfo;
        dish.Status = request.Status;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}