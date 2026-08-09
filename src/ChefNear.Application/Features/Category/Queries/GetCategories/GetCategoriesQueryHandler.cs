using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Features.Category.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Categories.Queries;

public class GetCategoriesQueryHandler
    : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CategoryDto>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var Categories = await _unitOfWork.Categories.GetAllAsync();

        return Result.Success(
    Categories
        .OrderBy(c => c.Name)
        .Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        })
        .ToList()
);
    }
}