namespace ChefNear.Domain.Enums;

public enum PaymentStatus
{
    Pending = 1,
    Held = 2,
    Released = 3,
    RefundInProgress = 4,
    Refunded = 5,
    Failed = 6
}
