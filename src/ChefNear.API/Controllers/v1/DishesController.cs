using Asp.Versioning;
using ChefNear.Application.Features.Dish.Queries.GetDishByIdQuery;
using ChefNear.Application.Features.Dishes.Commands;
using ChefNear.Application.Features.Dishes.Queries.GetNearbyDishesQuery;
using ChefNear.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        var result = await Mediator.Send(new GetDishByIdQuery(id));

        return HandleResult(result);
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDishRequest request)
    {
        var chefId = Guid.Parse(GetUser().Id);

        var command = new CreateDishCommand(
            chefId,
            request.CategoryId,
            request.Name,
            request.Description,
            request.Price,
            request.QuantityAvailable,
            request.AllergenInfo,
            request.ImageUrls,
            request.Ingredients);

        var result = await Mediator.Send(command);

        return HandleResult(result, "Dish created successfully.");
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDishRequest request)
    {
        var chefId = Guid.Parse(GetUser().Id);

        var command = new UpdateDishCommand(
            id,
            chefId,
            request.CategoryId,
            request.Name,
            request.Description,
            request.Price,
            request.QuantityAvailable,
            request.AllergenInfo,
            request.Status);

        var result = await Mediator.Send(command);

        return HandleResult(result, "Dish updated successfully.");
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var chefId = GetUser().Id;

        var result = await Mediator.Send(new DeleteDishCommand(id, chefId));

        return HandleResult(result, "Dish deleted successfully.");
    }
}