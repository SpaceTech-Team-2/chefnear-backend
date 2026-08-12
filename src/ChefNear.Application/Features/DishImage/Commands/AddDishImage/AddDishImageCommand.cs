using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ChefNear.Application.Features.DishImages.Commands.AddDishImage;

public record AddDishImageRequest(
    Guid DishId,
    IFormFile File,
    bool IsPrimary
);

public record AddDishImageCommand(
    Guid DishId,
    string ChefId,
    IFormFile File,
    bool IsPrimary
) : IRequest<Result<Guid>>;