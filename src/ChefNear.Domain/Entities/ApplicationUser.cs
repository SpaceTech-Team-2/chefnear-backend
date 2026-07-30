using Microsoft.AspNetCore.Identity;

namespace ChefNear.Domain.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
