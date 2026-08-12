using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.DishImages.Commands.RemoveDishImage;

public record RemoveDishImageCommand(
    Guid ImageId,
    string ChefId
) : IRequest<Result>;