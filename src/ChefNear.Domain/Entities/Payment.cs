using ChefNear.Domain.Common;
using HomeChefMarketplace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Domain.Entities
{
    
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? GatewayTransactionId { get; set; }

        public DateTime? PaidAt { get; set; }
        public DateTime? HeldAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
    }

}
