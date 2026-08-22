using Asp.Versioning;
using ChefNear.Application.Features.Orders.Commands.AcceptOrder;
using ChefNear.Application.Features.Orders.Commands.CancelOrder;
using ChefNear.Application.Features.Orders.Commands.MarkAsDelivered;
using ChefNear.Application.Features.Orders.Commands.MarkAsReady;
using ChefNear.Application.Features.Orders.Commands.PlaceOrder;
using ChefNear.Application.Features.Orders.Commands.StartPreparing;
using ChefNear.Application.Features.Orders.DTOs;
using ChefNear.Application.Features.Orders.Queries.GetChefOrders;
using ChefNear.Application.Features.Orders.Queries.GetOrderById;
using ChefNear.Shared.Constants;
using ChefNear.Shared.Responses;
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
    [HttpPost("checkout")]
    [Authorize(Roles = UserRoles.Client)]
    [ProducesResponseType<ApiResponse<PlaceOrderResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Checkout([FromBody] PlaceOrderRequest request)
    {
        var command = new PlaceOrderCommand(
            request.IdempotencyKey,
            request.Items,
            request.Notes,
            request.DeliveryAddressId,
            request.DeliveryAddress,
            request.PaymentGateway,
            request.OrderFulfillmentType,
            GetUser()
        );

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/accept")]
    [Authorize(Roles = UserRoles.Chef)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Accept(
        [FromRoute, SwaggerParameter("The unique identifier of the order.")] Guid id)
    {
        var command = new AcceptOrderCommand(id, GetUser());

        var result = await Mediator.Send(command);

        return HandleResult(result);
    }

    [HttpPut("{id:guid}/mark-as-ready")]
    [Authorize(Roles = UserRoles.Chef)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
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

    [HttpPut("{id:guid}/mark-as-delivered")]
    [Authorize(Roles = UserRoles.Client)]
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

    [HttpPut("{id:guid}/cancel")]
    [Authorize]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        [FromRoute] Guid id,
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

    [HttpPut("{id:guid}/start-preparing")]
    [Authorize(Roles = UserRoles.Chef)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartPreparing(
        [FromRoute] Guid id,
        [FromBody] StartOrderPreparingRequest request)
    {
        var command = new StartOrderPreparingCommand(
            id,
            GetUser(),
            request.EstimatedCookingTime);

        var result = await Mediator.Send(command);

        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType<ApiResponse<GetOrderByIdDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var query = new GetOrderByIdQuery(id, GetUser());

        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Chef)]
    [ProducesResponseType<ApiResponse<List<ChefOrderDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChefOrders([FromQuery] GetChefOrdersRequest request)
    {
        var query = new GetChefOrdersQuery(
            GetUser(),
            request.IsActive,
            request.PageNumber,
            request.PageSize
        );

        var result = await Mediator.Send(query);
        return HandleResult(result);
    }
}