using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.CreateCategory;

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.categories.GetAllAsync();

        var categoryExists = categories.Any(c => c.Name == request.Name);

        if (categoryExists)
        {
            return Error.Conflict(
      "Category.Exists",
      "A category with this name already exists."
  );
        }

        var category = new Domain.Entities.Category
        {
            Name = request.Name,
            Description = request.Description
        };

        await _unitOfWork.categories.AddAsync(category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(category.Id);
    }
}