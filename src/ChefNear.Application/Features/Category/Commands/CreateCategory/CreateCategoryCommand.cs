using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}