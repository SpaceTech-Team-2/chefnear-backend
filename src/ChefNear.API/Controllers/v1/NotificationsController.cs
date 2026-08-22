using Asp.Versioning;
using ChefNear.Application.Features.Notifications.Commands.ClearAllNotifications;
using ChefNear.Application.Features.Notifications.Commands.DeleteNotification;
using ChefNear.Application.Features.Notifications.Commands.RegisterDeviceToken;
using ChefNear.Application.Features.Notifications.DTOs;
using ChefNear.Application.Features.Notifications.Queries.GetNotifications;
using ChefNear.Shared.Responses;
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
    [ProducesResponseType<ApiResponse<List<NotificationDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        var user = GetUser();
        var query = new GetNotificationsQuery(user, request.PageNumber, request.PageSize);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNotification(Guid id)
    {
        var user = GetUser();
        var command = new DeleteNotificationCommand(id, user.Id);
        var result = await Mediator.Send(command);
        return HandleResult(result, "Notification deleted successfully.");
    }

    [HttpDelete("clear")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ClearAllNotifications()
    {
        var user = GetUser();
        var command = new ClearAllNotificationsCommand(user.Id);
        var result = await Mediator.Send(command);
        return HandleResult(result, "All notifications cleared successfully.");
    }

    [HttpPost("device-tokens")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterDeviceToken([FromBody] string token)
    {
        var command = new RegisterDeviceTokenCommand(token, GetUser().Id);

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}
