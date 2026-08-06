using ChefNear.Domain.Common;
using HomeChefMarketplace.Domain.Enums;
using System;

namespace ChefNear.Domain.Entities
{
    public class Payment : BaseEntity
    {       
        // Unique key to ensure idempotency of order creation requests
        public string IdempotencyKey { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? FailureReason { get; set; } 

        public string? GatewayTransactionId { get; set; }
        public string? RefundTransactionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? PaymentGatewayOrderId { get; set; }

        public DateTime? PaidAt { get; private set; }
        public DateTime? HeldAt { get; private set; }
        public DateTime? ReleasedAt { get; private set; }
        public DateTime? RefundedAt { get; private set; }

        public void Hold()
        {
            Status = PaymentStatus.Held;
            HeldAt = DateTime.UtcNow;
        }

        public void Release()
        {
            Status = PaymentStatus.Released;
            ReleasedAt = DateTime.UtcNow;
        }

        public void Refund(string? refundTransactionId = null)
        {
            Status = PaymentStatus.Refunded;
            RefundedAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(refundTransactionId))
                RefundTransactionId = refundTransactionId;
        }

        public void MarkAsFailed(string reason)
        {
            Status = PaymentStatus.Failed;
            FailureReason = reason;
        }
    }
}
