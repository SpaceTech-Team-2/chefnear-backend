using ChefNear.Domain.Common;

namespace ChefNear.Domain.Entities
{
    public class DeviceToken : BaseEntity
    {
        public User User { get; set; } = default!;
        public string UserId { get; private set; } = default!;

        public string Token { get; private set; } = string.Empty!;
        public bool IsActive { get; private set; }
    }
}
