using Asp.Versioning;
using ChefNear.Application.Features.Orders.Commands.AcceptOrder;
using ChefNear.Application.Features.Orders.Commands.CancelOrder;
using ChefNear.Application.Features.Orders.Commands.MarkAsDelivered;
using ChefNear.Application.Features.Orders.Commands.MarkAsReady;
using ChefNear.Application.Features.Orders.Commands.PlaceOrder;
using ChefNear.Application.Features.Orders.Commands.StartPreparing;
using ChefNear.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChefNear.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[SwaggerTag("Provides endpoints for placing orders and managing the order lifecycle.")]
public class OrdersController : BaseApiController
{
    /// <summary>
    /// Places a new order.
    /// </summary>
    /// <remarks>
    /// <b>Allowed Roles:</b> Client
    ///
    /// Creates a new order for the authenticated client and initiates the checkout process.
    /// </remarks>
    [HttpPost("checkout")]
    [Authorize(Roles = UserRoles.Client)]
    [SwaggerOperation(
        Summary = "Place a new order",
        Description = """
        **Allowed Roles:** Client

        Creates a new order and initiates the checkout/payment process.
        """,
        OperationId = "Orders_Checkout",
        Tags = new[] { "Orders" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Checkout([FromBody] PlaceOrderCommand command)
    {
        command.Client = GetUser();

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Accepts a pending order.
    /// </summary>
    /// <remarks>
    /// <b>Allowed Roles:</b> Chef
    ///
    /// Accepts a pending order assigned to the authenticated chef.
    /// </remarks>
    [HttpPut("{id:guid}/accept")]
    [Authorize(Roles = UserRoles.Chef)]
    [SwaggerOperation(
        Summary = "Accept an order",
        Description = """
        **Allowed Roles:** Chef

        Accepts a pending order assigned to the authenticated chef.
        """,
        OperationId = "Orders_Accept",
        Tags = new[] { "Orders" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Accept(
        [FromRoute, SwaggerParameter("The unique identifier of the order.")] Guid id)
    {
        var command = new AcceptOrderCommand(id, GetUser());

        var result = await Mediator.Send(command);

        return HandleResult(result);
    }

    /// <summary>
    /// Marks an order as ready.
    /// </summary>
    /// <remarks>
    /// <b>Allowed Roles:</b> Chef
    ///
    /// Changes the order status to <c>Ready</c> and records the estimated delivery time.
    /// </remarks>
    [HttpPut("{id:guid}/mark-as-ready")]
    [Authorize(Roles = UserRoles.Chef)]
    [SwaggerOperation(
        Summary = "Mark an order as ready",
        Description = """
        **Allowed Roles:** Chef

        Changes the order status to Ready and records the estimated delivery time.
        """,
        OperationId = "Orders_MarkAsReady",
        Tags = new[] { "Orders" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsReady(
        [FromRoute, SwaggerParameter("The unique identifier of the order.")] Guid id,
        [FromBody] MarkOrderAsReadyRequest request)
    {
        var command = new MarkOrderAsReadyCommand(
            id,
            GetUser(),
            request.EstimatedDeliveryTime);

        var result = await Mediator.Send(command);

        return HandleResult(result);
    }

    /// <summary>
    /// Marks an order as delivered.
    /// </summary>
    /// <remarks>
    /// <b>Allowed Roles:</b> Client
    ///
    /// Confirms that the client has received the order.
    /// This action completes the order lifecycle.
    /// </remarks>
    [HttpPut("{id:guid}/mark-as-delivered")]
    [Authorize(Roles = UserRoles.Client)]
    [SwaggerOperation(
        Summary = "Mark an order as delivered",
        Description = """
        **Allowed Roles:** Client

        Confirms that the client has received the order.

        This action completes the order lifecycle and may trigger
        post-delivery processes such as releasing the chef's earnings.
        """,
        OperationId = "Orders_MarkAsDelivered",
        Tags = new[] { "Orders" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsDelivered(
        [FromRoute, SwaggerParameter("The unique identifier of the order.")] Guid id)
    {
        var command = new MarkAsDeliveredCommand(id, GetUser());

        var result = await Mediator.Send(command);

        return HandleResult(result);
    }
}