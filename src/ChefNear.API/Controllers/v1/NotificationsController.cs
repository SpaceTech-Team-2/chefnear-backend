using Asp.Versioning;
using ChefNear.Application.Features.Notifications.Commands;
using ChefNear.Application.Features.Notifications.Queries.GetNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
[Produces("application/json")]
[Consumes("application/json")]
public class NotificationsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        var user = GetUser();
        var query = new GetNotificationsQuery(user, request.PageNumber, request.PageSize);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteNotification(Guid id)
    {
        var user = GetUser();
        var command = new DeleteNotificationCommand(id, user.Id);
        var result = await Mediator.Send(command);
        return HandleResult(result, "Notification deleted successfully.");
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearAllNotifications()
    {
        var user = GetUser();
        var command = new ClearAllNotificationsCommand(user.Id);
        var result = await Mediator.Send(command);
        return HandleResult(result, "All notifications cleared successfully.");
    }
}
