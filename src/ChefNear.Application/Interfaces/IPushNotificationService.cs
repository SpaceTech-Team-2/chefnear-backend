namespace ChefNear.Application.Interfaces;

public interface IPushNotificationService
{
    Task<string> PushNotificationAsync(string token, string title, string body, IReadOnlyDictionary<string, string> data = null);
}
