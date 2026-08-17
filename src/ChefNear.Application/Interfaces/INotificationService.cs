using ChefNear.Domain.Enums;
using HomeChefMarketplace.Domain.Enums;

namespace ChefNear.Application.Interfaces;

public interface INotificationService
{
    Task SendAsync(string userId, Guid notificationId, string message, NotificationType type, Dictionary<string, string>? additionalData = null);   
}
