using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Category.DTOs;
using MediatR;

namespace ChefNear.Application.Features.Categories.Queries;

public class GetCategoriesQueryHandler
    : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CategoryDto>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.categories.GetAllAsync();

        return categories
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToList();
    }
}