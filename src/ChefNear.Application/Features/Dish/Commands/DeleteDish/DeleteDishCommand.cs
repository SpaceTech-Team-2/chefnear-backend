using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Dishes.Commands;

public class DeleteDishCommand : IRequest<Result>
{
    public Guid DishId { get; set; }

    public string ChefId { get; set; } = string.Empty;
}