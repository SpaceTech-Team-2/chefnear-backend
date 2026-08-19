using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Domain.Enums;
using HomeChefMarketplace.Domain.Enums;

namespace ChefNear.Application.Features.Orders.DTOs;

public class GetOrderByIdDto    
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public OrderFulfillmentType OrderFulfillmentType { get; set; }
    public TimeSpan? EstimatedDeliveryTime { get; set; }
    public TimeSpan? EstimatedCookingTime { get; set; }
    public CancelledBy? CancelledBy { get; set; }
    public CancellationReasonType? CancellationReasonType { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? DeliveryFee { get; set; }
    public string? Notes { get; set; }

    public Guid DeliveryAddressId { get; set; }
    public AddressDto Address { get; set; }

    public string ChefId { get; set; } = string.Empty;
    public string ChefName { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;

    public List<OrderItemDto> Items { get; set; } = new();
    
    public List<OrderTrackingDto> Tracking { get; set; } = new();
}

public class OrderTrackingDto
{
    public OrderStatus Status { get; set; }
    public bool Completed { get; set; }
    public DateTime? Timestamp { get; set; } 
}

public class OrderItemDto
{
    public Guid DishId { get; set; }
    public string DishName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }   
}
