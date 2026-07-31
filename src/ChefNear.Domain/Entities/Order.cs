using ChefNear.Domain.Common;
using HomeChefMarketplace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ChefNear.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string ClientId { get; set; } = string.Empty; 
        public User Client { get; set; } = null!;

        public Guid DishId { get; set; }
        public Dish Dish { get; set; } = null!;

        public Guid DeliveryAddressId { get; set; }
        public Address DeliveryAddress { get; set; } = null!;

        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

  
        public CancelledBy? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }

        public Payment? Payment { get; set; }
        public Review? Review { get; set; }
        public Dispute? Dispute { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
