using ChefNear.Application.Features.DishImage.Queries.GetDishImages;
using ChefNear.Application.Features.DishImages.Commands.AddDishImage;
using ChefNear.Application.Features.DishImages.Commands.RemoveDishImage;
using ChefNear.Application.Features.DishImages.Commands.SetPrimaryDishImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class DishImageController : ControllerBase
{
    private readonly IMediator _mediator;

    public DishImageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{dishId:guid}")]
    public async Task<IActionResult> GetImages(Guid dishId)
    {
        var result = await _mediator.Send(new GetDishImagesQuery
        {
            DishId = dishId
        });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [Authorize(Roles = "Chef")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddDishImageCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [Authorize(Roles = "Chef")]
    [HttpPut("primary")]
    public async Task<IActionResult> SetPrimary([FromBody] SetPrimaryDishImageCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }

    [Authorize(Roles = "Chef")]
    [HttpDelete("{imageId:guid}")]
    public async Task<IActionResult> Delete(Guid imageId, [FromQuery] Guid chefId)
    {
        var command = new RemoveDishImageCommand
        {
            ImageId = imageId,
            ChefId = chefId.ToString()
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }
}