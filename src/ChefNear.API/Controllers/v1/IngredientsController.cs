using Asp.Versioning;
using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Application.Features.Ingredints.Commands.AddIngredient;
using ChefNear.Application.Features.Ingredints.Commands.RemoveIngredient;
using ChefNear.Application.Features.Ingredints.Commands.UpdateIngredient;
using ChefNear.Application.Features.Ingredints.Queries.GetIngredientsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class IngredientsController : BaseApiController
{
    [HttpGet("{dishId:guid}")]
    public async Task<IActionResult> GetIngredients(Guid dishId)
    {
        var result = await Mediator.Send(
            new GetIngredientsQuery
            {
                DishId = dishId
            });

        return HandleResult(
            result,
            "Ingredients retrieved successfully.");
    }

    [Authorize(Roles = "Chef")]
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddIngredientCommand command)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.ChefId = userId;

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Ingredient added successfully.");
    }

    [Authorize(Roles = "Chef")]
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateIngredientCommand command)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.ChefId = userId;

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Ingredient updated successfully.");
    }

    [Authorize(Roles = "Chef")]
    [HttpDelete("{ingredientId:guid}")]
    public async Task<IActionResult> Delete(Guid ingredientId)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await Mediator.Send(
            new RemoveIngredientCommand
            {
                IngredientId = ingredientId,
                ChefId = userId
            });

        return HandleResult(
            result,
            "Ingredient removed successfully.");
    }
}