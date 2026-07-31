namespace HomeChefMarketplace.Domain.Enums;

public enum UserRole
{
    Client = 1,
    Chef = 2,
    Admin = 3
}

public enum UserStatus
{
    Active = 1,
    Suspended = 2
}

public enum DishStatus
{
    Available = 1,
    Unavailable = 2,
    RemovedByAdmin = 3
}

// FR-12: Pending -> Accepted -> Preparing -> Ready for Delivery -> Delivered -> Cancelled
public enum OrderStatus
{
    Pending = 1,
    Accepted = 2,
    Preparing = 3,
    ReadyForDelivery = 4,
    Delivered = 5,
    Cancelled = 6
}

public enum CancelledBy
{
    Client = 1,
    Chef = 2
}

// Full amount is charged once the chef accepts, held in escrow,
// and released the instant the client confirms delivery (no weekly batching).
public enum PaymentStatus
{
    Pending = 1,
    Held = 2,
    Released = 3,
    Refunded = 4
}

public enum DisputeType
{
    DeliveryDispute = 1,   // FR-21: chef files, client didn't confirm delivery
    MissingOrderReport = 2 // FR-22: client files, order never arrived
}

public enum DisputeStatus
{
    Open = 1,
    UnderReview = 2,
    Resolved = 3,
    Rejected = 4
}

public enum NotificationType
{
    OrderPlaced = 1,
    OrderAccepted = 2,
    OrderPreparing = 3,
    OrderReady = 4,
    OrderDelivered = 5,
    OrderCancelled = 6,
    RefundProcessed = 7
}

public enum NotificationStatus
{
    Pending = 1,
    Sent = 2,
    Failed = 3
}
