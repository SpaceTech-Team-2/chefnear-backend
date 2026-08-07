using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public class DeleteDishCommandHandler : IRequestHandler<DeleteDishCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDishCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        var dish = await _unitOfWork.Dishes.GetByIdAsync(request.DishId);

        if (dish == null || dish.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("Dish.NotFound", "Dish not found.")
            );
        }

        if (dish.ChefId != request.ChefId)
        {
            return Result.Failure(
                Error.Forbidden("Dish.Forbidden", "Only the owning chef can delete this dish.")
            );
        }

        dish.IsDeleted = true;
        dish.DeletedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}