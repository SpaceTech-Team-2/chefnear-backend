using Asp.Versioning;
using ChefNear.API.Controllers;
using ChefNear.Application.Features.Categories.Queries;
using ChefNear.Application.Features.Category.Commands.CreateCategory;
using ChefNear.Application.Features.Category.Commands.DeleteCategory;
using ChefNear.Application.Features.Category.Commands.UpdateCategory;
using ChefNear.Application.Features.Category.DTOs;
using ChefNear.Shared.Constants;
using ChefNear.Shared.Responses;
using HomeChefMarketplace.Domain.Enums;
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
    [ProducesResponseType<ApiResponse<List<CategoryDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetCategoriesQuery());
        return HandleResult(result);
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost]
    [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryCommand command)
    {
        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Category created successfully.");
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPut("{categoryId:guid}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid categoryId,
        [FromBody] UpdateCategoryRequest request)
    {
        var command = new UpdateCategoryCommand(
            categoryId,
            request.Name,
            request.Description);

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Category updated successfully.");
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{categoryId:guid}")]
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        var result = await Mediator.Send(new DeleteCategoryCommand(categoryId));

        return HandleResult(
            result,
            "Category deleted successfully.");
    }
}