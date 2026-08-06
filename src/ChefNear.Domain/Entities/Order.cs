using ChefNear.Domain.Common;
using ChefNear.Domain.Enums;
using HomeChefMarketplace.Domain.Enums;

namespace ChefNear.Domain.Entities
{
    public class Order : BaseEntity , ISoftDelete
    {
        public string ClientId { get; set; } = string.Empty; 
        public Client Client { get; set; } = null!;

        public string ChefId { get; set; } = string.Empty;
        public Chef Chef { get; set; } = default!;

        public Guid DeliveryAddressId { get; set; }
        public Address DeliveryAddress { get; set; } = null!;

        public string? Notes { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
  
        public CancelledBy? CancelledBy { get; set; }
        public CancellationReasonType? CancellationReasonType { get; set; }
        public string? CancellationReason { get; set; }

        public TimeSpan? EstimatedDeliveryTime { get; set; }
        public TimeSpan? EstimatedCookingTime { get; set; }

        public Payment? Payment { get; set; }
        public Review? Review { get; set; }
        public Dispute? Dispute { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
