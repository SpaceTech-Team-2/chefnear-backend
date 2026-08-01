using ChefNear.Application.Features.Category.DTOs;
using MediatR;

namespace ChefNear.Application.Features.Categories.Queries;

public class GetCategoriesQuery : IRequest<List<CategoryDto>>
{
}