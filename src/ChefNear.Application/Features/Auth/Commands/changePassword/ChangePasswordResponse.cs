namespace ChefNear.Application.Features.Auth.Commands.changePassword;

public class ChangePasswordResponse
{
    public string Id { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
}
