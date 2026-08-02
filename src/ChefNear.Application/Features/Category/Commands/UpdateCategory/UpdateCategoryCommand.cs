using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<Result>
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}