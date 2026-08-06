using Microsoft.AspNetCore.Identity;
using HomeChefMarketplace.Domain.Enums;
using System.Collections.Generic;

namespace ChefNear.Domain.Entities
{
    public class User : IdentityUser 
    {
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? PhotoUrl { get; set; }

        // ---- Navigation ----
        public ICollection<Dispute> FiledDisputes { get; set; } = new List<Dispute>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}