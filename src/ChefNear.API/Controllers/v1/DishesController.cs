using Asp.Versioning;
using ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery;
using ChefNear.Application.Features.Dishes.Commands;
using ChefNear.Application.Features.Dishes.Queries.GetNearbyDishesQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefNear.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class DishesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetNearbyDishes(
        [FromQuery] GetNearbyDishesQuery query)
    {
        var result = await Mediator.Send(query);

        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(
            new GetDishByIdQuery
            {
                DishId = id
            });

        return HandleResult(result);
    }

    [Authorize(Roles = "Chef")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDishCommand command)
    {
        command.ChefId = Guid.Parse(GetUser().Id);
        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Dish created successfully.");
    }

    [Authorize(Roles = "Chef")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateDishCommand command)
    {
        if (id != command.DishId)
        {
            return BadRequest(new
            {
                message = "Route id does not match request body."
            });
        }
        command.ChefId = Guid.Parse(GetUser().Id);

        var result = await Mediator.Send(command);

        return HandleResult(result,"Dish updated successfully.");
    }

    [Authorize(Roles = "Chef")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var chefId = Guid.Parse(GetUser().Id);


        if (chefId == null)
        {
            return Unauthorized(new
            {
                message = "Chef id claim is missing."
            });
        }

        var result = await Mediator.Send(
            new DeleteDishCommand
            {
                DishId = id,
                ChefId = chefId.ToString()
            });

        return HandleResult(
            result,
            "Dish deleted successfully.");
    }
}