using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace ChefNear.Infrastructure.Hubs;

public class NotificationHub(IUnitOfWork unitOfWork) : Hub
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task MarkAsReceived(Guid notificationId)
    {
        var notification = await unitOfWork.Notifications.GetByIdAsync(notificationId);

        if (notification.Status == NotificationStatus.Received)
            return;

        notification.Status = NotificationStatus.Received;
        await unitOfWork.SaveChangesAsync();
    }
}
