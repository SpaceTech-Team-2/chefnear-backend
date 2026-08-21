using ChefNear.Application.Common.Jobs;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using ChefNear.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ChefNear.Infrastructure.Jobs;

internal class PushNotificationJob(
    IUnitOfWork unitOfWork,
    IPushNotificationService pushNotificationService,
    ILogger<PushNotificationJob> logger) : IPushNotificationJob
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPushNotificationService _pushNotificationService = pushNotificationService;
    private readonly ILogger<PushNotificationJob> _logger = logger;

    public async Task ExecuteAsync(Guid notificationId)
    {
        var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);

        if (notification == null)
            return;

        if (notification.Status == NotificationStatus.Received)
            return;

        var userDeviceTokens = await _unitOfWork.DeviceTokens.GetByUserIdAsync(notification.UserId);

        if (!userDeviceTokens.Any())
            return;

        var failedTokens = new List<string>();

        foreach (var token in userDeviceTokens)
        {
            try
            {
                var messageId = await _pushNotificationService.PushNotificationAsync(
                    token,
                    notification.Title,
                    notification.Message,
                    new Dictionary<string, string>
                    {
                        {"orderId", notification.OrderId.ToString() }
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Exception thrown while push notification for Token: {Token}, to User: {UserId}, Notification: {@Notification}",
                    token,
                    notification.UserId,
                    notification
                );

                failedTokens.Add(token);
            }
        }
        if (failedTokens.Any())
            _logger.LogWarning("Faliled To Push notifications for DeviceTokens: {@DeviceTokens} for UserId: {UserID}",
                failedTokens,
                notification.UserId

            );
    }
}
