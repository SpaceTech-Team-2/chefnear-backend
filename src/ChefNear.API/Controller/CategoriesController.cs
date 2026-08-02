using ChefNear.Application.Features.Categories.Queries;
using ChefNear.Application.Features.Category.Commands.CreateCategory;
using ChefNear.Application.Features.Category.Commands.DeleteCategory;
using ChefNear.Application.Features.Category.Commands.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCategoriesQuery());

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error.Description);

            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId:guid}")]
        public async Task<IActionResult> Update(Guid categoryId, [FromBody] UpdateCategoryCommand command)
        {
            if (categoryId != command.CategoryId)
                return BadRequest("Route categoryId does not match request body.");

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error.Description);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var command = new DeleteCategoryCommand
            {
                CategoryId = categoryId
            };

            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error.Description);

            return Ok();
        }
    }
}