using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);

        if (category == null)
        {
            return Result.Failure(
                Error.NotFound("Category.NotFound", "Category not found.")
            );
        }

        var Categories = await _unitOfWork.Categories.GetAllAsync();

        var nameTaken = Categories.Any(c =>
            c.Id != request.CategoryId &&
            c.Name == request.Name);

        if (nameTaken)
        {
            return Result.Failure(
                Error.Conflict(
                    "Category.Exists",
                    "A category with this name already exists."
                )
            );
        }

        category.Name = request.Name;
        category.Description = request.Description;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}