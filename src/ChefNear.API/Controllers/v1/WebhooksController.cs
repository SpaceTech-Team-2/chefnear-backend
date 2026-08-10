using ChefNear.API.Filters;
using ChefNear.Application.Common.Payments.Paymob;
using ChefNear.Application.Features.Orders.Commands.ProcessPaymobWebhook;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1;

[Route("[controller]")]
[ApiController]
public class WebhooksController : BaseApiController
{
    [HttpPost("paymob")]
    [VerifyHmac]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> PaymobWebhook([FromBody] PaymobWebhook paymobWebhook)
    {
        var command = new ProcessPaymobWebhookCommand(paymobWebhook);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
