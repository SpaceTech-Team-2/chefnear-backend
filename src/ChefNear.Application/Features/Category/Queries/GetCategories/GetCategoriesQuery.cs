using ChefNear.Application.Features.Category.DTOs;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Categories.Queries;

public record GetCategoriesQuery() : IRequest<Result<List<CategoryDto>>>;