using BenchmarkDotNet.Reports;
using ChefNear.Application.Features.Orders.Commands.CancelOrder;
using ChefNear.Application.Features.Orders.Commands.StartPreparing;
using ChefNear.Application.Model;
using ChefNear.Shared.Constants;
using ChefNear.Shared.Responses;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace ChefNear.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected CurrentUser GetUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var firstName = User.FindFirst("FName")?.Value;
        var lastName = User.FindFirst("LName")?.Value;
        var phoneNumber = User.FindFirst("PhoneNumber")?.Value;

        if (userId == null || email == null || role == null || firstName == null || lastName == null || phoneNumber == null)
            throw new UnauthorizedAccessException("User information is incomplete or missing.");

        return new CurrentUser
        {
            Id = userId,
            Email = email,
            Role = role,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber
        };
    }

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
