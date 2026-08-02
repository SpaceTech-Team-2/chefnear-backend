using ChefNear.Application.Features.Dish.DTOs;
using ChefNear.Application.Features.Ingredints.Commands.AddIngredient;
using ChefNear.Application.Features.Ingredints.Commands.RemoveIngredient;
using ChefNear.Application.Features.Ingredints.Commands.UpdateIngredient;
using ChefNear.Application.Features.Ingredints.Queries.GetIngredientsQuery;
using ChefNear.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IngredientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{dishId:guid}")]
    public async Task<IActionResult> GetIngredients(Guid dishId)
    {
        var result = await _mediator.Send(new GetIngredientsQuery
        {
            DishId = dishId
        });

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse.FailureResponse(
                result.Error.Description,
                result.Error.Description));
        }

        return Ok(ApiResponse<List<IngredientDtos>>.SuccessResponse(
            result.Value,
            "Ingredients retrieved successfully."));
    }

    [Authorize(Roles = "Chef")]
    [HttpPost]
    public async Task<IActionResult> Add(AddIngredientCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse.FailureResponse(
                result.Error.Description,
                result.Error.Description));
        }

        return Ok(ApiResponse<Guid>.SuccessResponse(
            result.Value,
            "Ingredient added successfully."));
    }

    [Authorize(Roles = "Chef")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateIngredientCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse.FailureResponse(
                result.Error.Description,
                result.Error.Description));
        }

        return Ok(ApiResponse.SuccessResponse(
            "Ingredient updated successfully."));
    }

    [Authorize(Roles = "Chef")]
    [HttpDelete("{ingredientId:guid}")]
    public async Task<IActionResult> Delete(Guid ingredientId, [FromQuery] Guid chefId)
    {
        var result = await _mediator.Send(new RemoveIngredientCommand
        {
            IngredientId = ingredientId,
            ChefId = chefId.ToString()
        });

        if (result.IsFailure)
        {
            return BadRequest(ApiResponse.FailureResponse(
                result.Error.Description,
                result.Error.Description));
        }

        return Ok(ApiResponse.SuccessResponse(
            "Ingredient removed successfully."));
    }
}