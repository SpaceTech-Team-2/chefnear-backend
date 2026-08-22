using Asp.Versioning;
using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Application.Features.DishImage.Queries.GetDishImages;
using ChefNear.Application.Features.DishImages.Commands.AddDishImage;
using ChefNear.Application.Features.DishImages.Commands.RemoveDishImage;
using ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage;
using ChefNear.Shared.Constants;
using ChefNear.Shared.Responses;
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
    [ProducesResponseType<ApiResponse<List<DishImageDto>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetImages(Guid dishId)
    {
        var result = await Mediator.Send(new GetDishImagesQuery(dishId));
        return HandleResult(result);
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpPost]
    [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid imageId)
    {
        var chefId = GetUser().Id;

        var command = new RemoveDishImageCommand(imageId, chefId);

        var result = await Mediator.Send(command);
        return HandleResult(result, "Dish image deleted successfully.");
    }
}