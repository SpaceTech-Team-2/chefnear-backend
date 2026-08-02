using ChefNear.Shared.Responses;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    /// <summary>
    /// Converts a non-generic Result into an appropriate HTTP response wrapped in ApiResponse.
    /// </summary>
    protected IActionResult HandleResult(Result result, string successMessage = "Operation completed successfully.")
    {
        if (result.IsSuccess)
            return Ok(ApiResponse.SuccessResponse(successMessage));

        return MapErrorToResponse(result.Error);
    }

    /// <summary>
    /// Converts a generic Result&lt;T&gt; into an appropriate HTTP response wrapped in ApiResponse&lt;T&gt;.
    /// </summary>
    protected IActionResult HandleResult<T>(Result<T> result, string successMessage = "Operation completed successfully.")
    {
        if (result.IsSuccess)
            return Ok(ApiResponse<T>.SuccessResponse(result.Value, successMessage));

        return MapErrorToResponse(result.Error);
    }

    private IActionResult MapErrorToResponse(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var response = ApiResponse.FailureResponse(error.Description, error.Code, statusCode);
        return StatusCode(statusCode, response);
    }
}
