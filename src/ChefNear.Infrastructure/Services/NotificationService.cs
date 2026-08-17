using ChefNear.Application.Interfaces;
using ChefNear.Domain.Enums;
using ChefNear.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChefNear.Infrastructure.Services;

internal class NotificationMessage
{
    public Guid Id { get; set; }    
    public string Message { get; set; } = string.Empty!;
    public NotificationType Type { get; set; }
    public Dictionary<string,string> additionalData { get; set; }   
}

internal class NotificationService(IHubContext<NotificationHub> hub) : INotificationService
{
    private readonly IHubContext<NotificationHub> _hub = hub;

    public async Task SendAsync(string userId, Guid notificationId ,string message, NotificationType type, Dictionary<string,string> additionalData = null)
    {
        if(string.IsNullOrEmpty(userId))
            throw new ArgumentNullException("userId is required to send notification.");

        if (notificationId == Guid.Empty)
            throw new ArgumentNullException("notificationId is required to send notificaiton.");

        var notification = new NotificationMessage
        {
            Id = notificationId,
            Message = message,
            Type = type,
            additionalData = additionalData
        };

        await _hub.Clients.User(userId).SendAsync("NotificationReceived", notification);
    }
}
