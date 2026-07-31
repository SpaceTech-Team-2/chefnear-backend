using ChefNear.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsActive => RevokedAt == null && ExpiresAt > DateTime.UtcNow;
    }
}
