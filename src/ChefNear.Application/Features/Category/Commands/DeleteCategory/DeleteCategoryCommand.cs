using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Category.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : IRequest<Result>;