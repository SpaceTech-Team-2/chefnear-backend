using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage;

public record SetPrimaryDishImageRequest(Guid ImageId);

public record SetPrimaryDishImageCommand(
    Guid ImageId,
    string ChefId
) : IRequest<Result>;