using ChefNear.Application.Common.Jobs;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Enums;
using ChefNear.Infrastructure.Hubs;
using Hangfire;
using Microsoft.AspNetCore.SignalR;

namespace ChefNear.Infrastructure.Services;

internal class NotificationMessage
{
    public Guid Id { get; set; }    
    public string Message { get; set; } = string.Empty!;
    public string Title { get; set; } = string.Empty!;
    public string Type { get; set; }
    public Dictionary<string,string> additionalData { get; set; }   
}

internal class NotificationService(
    IHubContext<NotificationHub> hub,
    IPushNotificationJob pushNotificationJob, 
    IBackgroundJobClient backgroundJobClient) : INotificationService
{
    private readonly IPushNotificationJob _pushNotificationJob = pushNotificationJob;
    private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
    private readonly IHubContext<NotificationHub> _hub = hub;

    public async Task SendAsync(string userId, Guid notificationId, string title, string message, NotificationType type, Dictionary<string, string> additionalData = null)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException("userId is required to send notification.");

        if (notificationId == Guid.Empty)
            throw new ArgumentNullException("notificationId is required to send notificaiton.");

        var notification = new NotificationMessage
        {
            Id = notificationId,
            Title = title,
            Message = message,
            Type = type.ToString(),
            additionalData = additionalData
        };

        await _hub.Clients.User(userId).SendAsync("NotificationReceived", notification);

        // Enque Push Notificaiton Job
        _backgroundJobClient
            .Schedule(() => _pushNotificationJob.ExecuteAsync(notificationId), TimeSpan.FromMinutes(2));
    }
}
