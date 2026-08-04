using ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery;
using ChefNear.Application.Features.Dishes.Commands;
using ChefNear.Application.Features.Dishes.Queries.GetNearbyDishesQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefNear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DishesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetNearbyDishes([FromQuery] GetNearbyDishesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetDishByIdQuery { DishId = id });
        if (result == null)
            return NotFound(new { message = "Dish not found." });

        return Ok(result);
    }

    [Authorize(Roles = "Chef")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateDishCommand command)
    {
        var result = await _mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { message = "Dish created successfully.", dishId = result.Value });
    }

    [Authorize(Roles = "Chef")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateDishCommand command)
    {
        if (id != command.DishId)
            return BadRequest(new { message = "Route id does not match request body." });

        var result = await _mediator.Send(command);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { message = "Dish updated successfully." });
    }

    [Authorize(Roles = "Chef")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var chefId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (chefId == null)
            return Unauthorized(new { message = "Chef id claim is missing." });

        var result = await _mediator.Send(new DeleteDishCommand
        {
            DishId = id,
            ChefId = chefId
        });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { message = "Dish deleted successfully." });
    }
}