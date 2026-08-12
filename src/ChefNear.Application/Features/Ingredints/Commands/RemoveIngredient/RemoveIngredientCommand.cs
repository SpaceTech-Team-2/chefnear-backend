using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Ingredints.Commands.RemoveIngredient;

public record RemoveIngredientCommand(
    Guid IngredientId,
    string ChefId
) : IRequest<Result>;
