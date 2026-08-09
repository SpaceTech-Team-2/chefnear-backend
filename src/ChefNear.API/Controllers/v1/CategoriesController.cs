using Asp.Versioning;
using ChefNear.API.Controllers;
using ChefNear.Application.Features.Categories.Queries;
using ChefNear.Application.Features.Category.Commands.CreateCategory;
using ChefNear.Application.Features.Category.Commands.DeleteCategory;
using ChefNear.Application.Features.Category.Commands.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Consumes("application/json")]
public class CategoriesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetCategoriesQuery());

        return HandleResult(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryCommand command)
    {
        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Category created successfully.");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{categoryId:guid}")]
    public async Task<IActionResult> Update(
        Guid categoryId,
        [FromBody] UpdateCategoryCommand command)
    {
        if (categoryId != command.CategoryId)
        {
            return BadRequest(new
            {
                message = "Route categoryId does not match request body."
            });
        }

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Category updated successfully.");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{categoryId:guid}")]
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        var result = await Mediator.Send(
            new DeleteCategoryCommand
            {
                CategoryId = categoryId
            });

        return HandleResult(
            result,
            "Category deleted successfully.");
    }
}