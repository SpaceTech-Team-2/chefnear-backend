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
    Confirmed = 2,
    Accepted = 3,
    Preparing = 4,
    OutForDelivery = 5,
    Delivered = 6,
    Cancelled = 7
}

public enum CancelledBy
{
    Client = 1,
    Chef = 2
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
