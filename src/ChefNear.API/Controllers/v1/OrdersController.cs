using Asp.Versioning;
using ChefNear.Application.Features.Orders.Commands.CancelOrder;
using ChefNear.Application.Features.Orders.Commands.PlaceOrder;
using ChefNear.Shared.Responses;
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
    public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] CancelOrderCommand command)
    {
        command = command with { OrderId = id, User = GetUser() };

        var result = await Mediator.Send(command);
        return HandleResult(result, "Order cancelled successfully.");
    }
}
