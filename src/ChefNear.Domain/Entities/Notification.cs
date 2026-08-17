using ChefNear.Domain.Common;
using ChefNear.Domain.Enums;

namespace ChefNear.Domain.Entities
{
   
    public class Notification : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }

        public NotificationType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public DateTime? SentAt { get; set; }
    }

}
