namespace ChefNear.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenResponse
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenExpiration { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public bool OnboardingCompleted { get; set; }
    public int CurrentStep { get; set; }
}
