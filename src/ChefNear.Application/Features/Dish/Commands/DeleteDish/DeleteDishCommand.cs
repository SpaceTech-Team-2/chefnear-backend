using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public record DeleteDishCommand(
    Guid DishId,
    string ChefId
) : IRequest<Result>;