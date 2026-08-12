using Asp.Versioning;
using ChefNear.Application.Features.Ingredints.Commands.AddIngredient;
using ChefNear.Application.Features.Ingredints.Commands.RemoveIngredient;
using ChefNear.Application.Features.Ingredints.Commands.UpdateIngredient;
using ChefNear.Application.Features.Ingredints.Queries.GetIngredientsQuery;
using ChefNear.Shared.Constants;
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
        var result = await Mediator.Send(new GetIngredientsQuery(dishId));

        return HandleResult(result, "Ingredients retrieved successfully.");
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddIngredientRequest request)
    {
        var chefId = GetUser().Id;

        var command = new AddIngredientCommand(
            request.DishId,
            chefId,
            request.Name,
            request.Quantity);

        var result = await Mediator.Send(command);

        return HandleResult(result, "Ingredient added successfully.");
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateIngredientRequest request)
    {
        var chefId = GetUser().Id;

        var command = new UpdateIngredientCommand(
            request.IngredientId,
            chefId,
            request.Name,
            request.Quantity);

        var result = await Mediator.Send(command);

        return HandleResult(result, "Ingredient updated successfully.");
    }

    [Authorize(Roles = UserRoles.Chef)]
    [HttpDelete("{ingredientId:guid}")]
    public async Task<IActionResult> Delete(Guid ingredientId)
    {
        var chefId = GetUser().Id;

        var result = await Mediator.Send(
            new RemoveIngredientCommand(ingredientId, chefId));

        return HandleResult(result, "Ingredient removed successfully.");
    }
}