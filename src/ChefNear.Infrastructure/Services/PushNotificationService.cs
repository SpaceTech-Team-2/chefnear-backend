using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Application.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;

namespace ChefNear.Infrastructure.Services;

internal class PushNotificationService(ILogger<PushNotificationService> logger, IUnitOfWork unitOfWork) : IPushNotificationService
{
    private readonly ILogger<PushNotificationService> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<string> PushNotificationAsync(string token, string title, string body, IReadOnlyDictionary<string, string> data = null)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogWarning(
                "Push notification skipped because FCM token is null or empty.");

            throw new ArgumentException(
                "FCM token cannot be null or empty.",
                nameof(token));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Notification title cannot be null or empty.",
                nameof(title));
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            throw new ArgumentException(
                "Notification body cannot be null or empty.",
                nameof(body));
        }

        var messageData = new Dictionary<string, string>
        {
            ["click_action"] = "FLUTTER_NOTIFICATION_CLICK"
        };

        if (data is not null)
        {
            foreach (var item in data)
            {
                messageData[item.Key] = item.Value;
            }
        }

        var message = new Message
        {
            Fid = token,
            Notification = new Notification
            {
                Title = title,
                Body = body
            },
            Data = messageData
        };

        try
        {
            _logger.LogInformation(
                "Sending push notification. Token: {Token}, Title: {Title}",
                token,
                title);

            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

            _logger.LogInformation(
                "Push notification sent successfully. Token: {Token}, FCM MessageId: {MessageId}",
                token,
                response);

            return response;
        }
        catch (FirebaseMessagingException ex)
        {
            _logger.LogError(
                ex,
                "FCM failed to send push notification. " +
                "Token: {Token}, ErrorCode: {ErrorCode}, " +
                "MessagingErrorCode: {MessagingErrorCode}",
                token,
                ex.ErrorCode,
                ex.MessagingErrorCode);

            if (IsInvalidToken(ex))
            {
                _logger.LogWarning(
                    "FCM token is no longer valid and should be deactivated. " +
                    "Token: {Token}, ErrorCode: {ErrorCode}, MessagingErrorCode: {MessagingErrorCode}",
                    token,
                    ex.ErrorCode,
                    ex.MessagingErrorCode);

                var deviceToken = await _unitOfWork.DeviceTokens.GetByTokenAsync(token);
                deviceToken.Deactivate();
                await _unitOfWork.SaveChangesAsync();
            }

            throw;
        }
    }

    private static bool IsInvalidToken(FirebaseMessagingException ex)
    {
        return ex.MessagingErrorCode is
            MessagingErrorCode.Unregistered;
    }
}
