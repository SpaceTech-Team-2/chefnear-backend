using ChefNear.Domain.Enums;
using System;

namespace ChefNear.Application.Features.Notifications.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? SentAt { get; set; }
    }
}
