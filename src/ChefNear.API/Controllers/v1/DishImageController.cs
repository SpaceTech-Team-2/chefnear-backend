using Asp.Versioning;
using ChefNear.Application.Features.DishImage.Queries.GetDishImages;
using ChefNear.Application.Features.DishImages.Commands.AddDishImage;
using ChefNear.Application.Features.DishImages.Commands.RemoveDishImage;
using ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Consumes("application/json")]
public class DishImageController : BaseApiController
{


    [HttpGet("{dishId:guid}")]
    public async Task<IActionResult> GetImages(Guid dishId)
    {
        var result = await Mediator.Send(
            new GetDishImagesQuery
            {
                DishId = dishId
            });

        return HandleResult(result);
    }

    [Authorize(Roles = "Chef")]
    [HttpPost]
    public async Task<IActionResult> Add(
         [FromForm] AddDishImageCommand command)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.ChefId = userId;

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Dish image added successfully.");
    }

    [Authorize(Roles = "Chef")]
    [HttpPut("primary")]
    public async Task<IActionResult> SetPrimary(
        [FromBody] SetPrimaryDishImageCommand command)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.ChefId = userId;

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Primary image updated successfully.");
    }

    [Authorize(Roles = "Chef")]
    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> Delete(Guid imageId)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var command = new RemoveDishImageCommand
        {
            ImageId = imageId,
            ChefId = userId
        };

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Dish image deleted successfully.");
    }
}