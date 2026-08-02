namespace ChefNear.Application.Features.Auth.Commands.Logout;

public class LogoutResponse
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
