using ChefNear.Domain.Common;

namespace ChefNear.Domain.Entities
{
    public class DeviceToken : BaseEntity
    {
        public User User { get; set; } = default!;
        public string UserId { get; private set; } = default!;

        public string Token { get; private set; } = string.Empty!;
        public bool IsActive { get; private set; }

        private DeviceToken(string token, string userId, bool isActive = true)
        {
            Token = token;
            UserId = userId;
            IsActive = isActive;
        }

        public static DeviceToken CreateToken(string token, string userId, bool isActive = true) =>
            new(token, userId, isActive);

        public void AssignToUser(string userId)
        {
            UserId = userId;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
