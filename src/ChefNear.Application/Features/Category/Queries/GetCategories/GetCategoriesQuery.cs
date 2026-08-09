using ChefNear.Application.Features.Category.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Categories.Queries;

public class GetCategoriesQuery : IRequest<Result<List<CategoryDto>>>
{
}