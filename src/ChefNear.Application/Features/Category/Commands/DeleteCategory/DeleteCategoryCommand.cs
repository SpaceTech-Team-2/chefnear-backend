using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<Result>
{
    public Guid CategoryId { get; set; }
}