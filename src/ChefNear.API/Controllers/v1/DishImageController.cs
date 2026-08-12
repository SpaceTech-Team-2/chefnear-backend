using Asp.Versioning;
using ChefNear.Application.Features.DishImage.Queries.GetDishImages;
using ChefNear.Application.Features.DishImages.Commands.AddDishImage;
using ChefNear.Application.Features.DishImages.Commands.RemoveDishImage;
using ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage;
using ChefNear.Shared.Constants;
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
        var result = await Mediator.Send(new GetDishImagesQuery(dishId));

        return HandleResult(result);
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpPost]
    public async Task<IActionResult> Add(
         [FromForm] AddDishImageRequest request)
    {
        var chefId = GetUser().Id;

        var command = new AddDishImageCommand(
            request.DishId,
            chefId,
            request.File,
            request.IsPrimary);

        var result = await Mediator.Send(command);

        return HandleResult(result, "Dish image added successfully.");
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpPut("primary")]
    public async Task<IActionResult> SetPrimary(
        [FromBody] SetPrimaryDishImageRequest request)
    {
        var chefId = GetUser().Id;

        var command = new SetPrimaryDishImageCommand(request.ImageId, chefId);

        var result = await Mediator.Send(command);

        return HandleResult(result, "Primary image updated successfully.");
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> Delete(Guid imageId)
    {
        var chefId = GetUser().Id;

        var command = new RemoveDishImageCommand(imageId, chefId);

        var result = await Mediator.Send(command);

        return HandleResult(result, "Dish image deleted successfully.");
    }
}