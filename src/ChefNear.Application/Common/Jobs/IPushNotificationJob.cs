namespace ChefNear.Application.Common.Jobs;

public interface IPushNotificationJob
{
    Task ExecuteAsync(Guid notificationId); 
}
