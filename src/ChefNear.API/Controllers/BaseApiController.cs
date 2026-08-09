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

    /// <summary>
    /// Cancels an existing order.
    /// </summary>
    /// <remarks>
    /// <b>Allowed Roles:</b> Client, Chef
    ///
    /// Cancels an order according to the application's business rules.
    /// The authenticated user must be authorized to cancel the specified order.
    /// </remarks>
    [HttpPut("{id:guid}/cancel")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Cancel an order",
        Description = """
        **Allowed Roles:** Client, Chef

        Cancels an existing order.

        The authenticated user must satisfy the application's business rules
        to cancel the specified order.
        """,
        OperationId = "Orders_Cancel",
        Tags = new[] { "Orders" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        [FromRoute, SwaggerParameter("The unique identifier of the order.")] Guid id,
        [FromBody] CancelOrderRequest request)
    {
        var command = new CancelOrderCommand(
            id,
            request.ReasonType,
            request.ReasonFreeText,
            GetUser());

        var result = await Mediator.Send(command);

        return HandleResult(result, "Order cancelled successfully.");
    }

    /// <summary>
    /// Starts preparing an accepted order.
    /// </summary>
    /// <remarks>
    /// <b>Allowed Roles:</b> Chef
    ///
    /// Changes the order status to <c>Preparing</c> and stores the estimated cooking time.
    /// </remarks>
    [HttpPut("{id:guid}/start-preparing")]
    [Authorize(Roles = UserRoles.Chef)]
    [SwaggerOperation(
        Summary = "Start preparing an order",
        Description = """
        **Allowed Roles:** Chef

        Changes the order status to Preparing and records the estimated cooking time.
        """,
        OperationId = "Orders_StartPreparing",
        Tags = new[] { "Orders" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartPreparing(
        [FromRoute, SwaggerParameter("The unique identifier of the order.")] Guid id,
        [FromBody] StartOrderPreparingRequest request)
    {
        var command = new StartOrderPreparingCommand(
            id,
            GetUser(),
            request.EstimatedCookingTime);

        var result = await Mediator.Send(command);

        return HandleResult(result);
    }
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
