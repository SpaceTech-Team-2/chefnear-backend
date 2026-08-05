namespace ChefNear.Infrastructure.Settings;

public class PaymobSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string HMAC { get; set; } = string.Empty;
    public string WebhookRoute { get; set; } = string.Empty;    
    public Endpoints Endpoints { get; set; }  
}

public class Endpoints
{
    public string Intention { get; set; } = string.Empty;
}