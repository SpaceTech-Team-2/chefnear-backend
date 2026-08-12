using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.UpdateCategory;

public record UpdateCategoryRequest(
    string Name,
    string? Description
);

public record UpdateCategoryCommand(
    Guid CategoryId,
    string Name,
    string? Description
) : IRequest<Result>;