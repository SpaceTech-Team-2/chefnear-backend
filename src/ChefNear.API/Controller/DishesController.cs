
using ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery;
using ChefNear.Application.Features.Dishes.Commands;
using ChefNear.Application.Features.Dishes.Queries.GetNearbyDishesQuery;
using ChefNear.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            return NotFound();

        return Ok(result);
    }

    [Authorize(Roles = "Chef")]

    [HttpPost]
    public async Task<IActionResult> Create(Application.Features.Dishes.Commands.CreateDishCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    [Authorize(Roles = "Chef")]

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateDishCommand command)
    {
        if (id != command.DishId)
            return BadRequest();
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }
    [Authorize(Roles = "Chef")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var chefId = User.FindFirst("UserId")?.Value;

        if (chefId == null)
            return Unauthorized();

        var result = await _mediator.Send(new DeleteDishCommand
        {
            DishId = id,
            ChefId = chefId
        });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok();
    }
}