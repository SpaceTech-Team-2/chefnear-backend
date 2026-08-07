using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.categories.GetByIdAsync(request.CategoryId);

        if (category == null)
        {
            return Result.Failure(
                Error.NotFound("Category.NotFound", "Category not found.")
            );
        }

        var dishes = await _unitOfWork.dishes.GetAllAsync();

        var inUse = dishes.Any(d => d.CategoryId == request.CategoryId && !d.IsDeleted);

        if (inUse)
        {
            return Result.Failure(
                Error.Conflict(
                    "Category.InUse",
                    "This category is used by one or more dishes and cannot be deleted."
                )
            );
        }

        await _unitOfWork.categories.DeleteAsync(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}