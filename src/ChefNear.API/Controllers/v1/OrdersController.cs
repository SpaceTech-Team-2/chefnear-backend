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

namespace ChefNear.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrdersController : BaseApiController
{
    [HttpPost("checkout")]
    [Authorize]
    public async Task<IActionResult> Checkout([FromBody] PlaceOrderCommand command) 
    {
        command.Client = GetUser();

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] CancelOrderRequest request)
    {
        var command = new CancelOrderCommand(id, request.ReasonType, request.ReasonFreeText, GetUser());

        var result = await Mediator.Send(command);
        return HandleResult(result, "Order cancelled successfully.");
    }

    [HttpPut("{id:guid}/accept")]
    [Authorize(Roles = UserRoles.Chef)]
    public async Task<IActionResult> Accept([FromRoute] Guid id)
    {
        var command = new AcceptOrderCommand(id, GetUser());

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/mark-as-ready")]
    [Authorize(Roles = UserRoles.Chef)]
    public async Task<IActionResult> MarkAsReady([FromRoute] Guid id, [FromBody] MarkOrderAsReadyRequest request)
    {
        var command = new MarkOrderAsReadyCommand(id, GetUser(), request.EstimatedDeliveryTime);

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/start-preparing")]
    [Authorize(Roles = UserRoles.Chef)]
    public async Task<IActionResult> StartPreparing([FromRoute] Guid id, [FromBody] StartOrderPreparingRequest request)
    {
        var command = new StartOrderPreparingCommand(id, GetUser(), request.EstimatedCookingTime);

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/mark-as-delivered")]
    [Authorize(Roles = UserRoles.Client)]
    public async Task<IActionResult> MarkAsDelivered([FromRoute] Guid id)
    {
        var command = new MarkAsDeliveredCommand(id, GetUser());

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
