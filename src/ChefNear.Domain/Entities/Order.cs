using ChefNear.Domain.Common;
using ChefNear.Domain.Enums;
using HomeChefMarketplace.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ChefNear.Domain.Entities
{
    public class Order : BaseEntity, ISoftDelete
    {
        public string ClientId { get; set; } = string.Empty; 
        public Client Client { get; set; } = null!;

        public string ChefId { get; set; } = string.Empty;
        public Chef Chef { get; set; } = default!;

        public Guid DeliveryAddressId { get; set; }
        public Address DeliveryAddress { get; set; } = null!;

        public string? Notes { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
  
        public CancelledBy? CancelledBy { get; private set; }
        public CancellationReasonType? CancellationReasonType { get; private set; }
        public string? CancellationReason { get; private set; }

        public TimeSpan? EstimatedDeliveryTime { get; private set; }
        public TimeSpan? EstimatedCookingTime { get; private set; }

        public Payment? Payment { get; set; }
        public Review? Review { get; set; }
        public Dispute? Dispute { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public void Accept()
        {
            Status = OrderStatus.Accepted;
        }

        public void Confirm()
        {
            Status = OrderStatus.Confirmed;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void StartPreparing(TimeSpan? estimatedCookingTime = null)
        {
            Status = OrderStatus.Preparing;
            if (estimatedCookingTime.HasValue)
                EstimatedCookingTime = estimatedCookingTime.Value;
        }

        public void MarkAsReady(TimeSpan? estimatedDeliveryTime = null)
        {
            Status = OrderStatus.ReadyForDelivery;
            if (estimatedDeliveryTime.HasValue)
                EstimatedDeliveryTime = estimatedDeliveryTime.Value;
        }

        public void MarkAsDelivered()
        {
            Status = OrderStatus.Delivered;
        }

        public void Cancel(CancelledBy cancelledBy, CancellationReasonType? reasonType = null, string? reason = null)
        {
            Status = OrderStatus.Cancelled;
            CancelledBy = cancelledBy;
            CancellationReasonType = reasonType;
            CancellationReason = reason;
        }
    }
}
