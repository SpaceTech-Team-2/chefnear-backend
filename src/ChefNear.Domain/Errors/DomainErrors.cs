using ChefNear.Shared.ResultPattern;

namespace ChefNear.Domain.Errors;

public static class DomainErrors
{
    public static class Auth
    {
        public static readonly Error InvalidCredentials =
            Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password.");

        public static readonly Error EmailNotConfirmed =
            Error.Unauthorized("Auth.EmailNotConfirmed", "Please confirm your email before logging in.");

        public static readonly Error AccountNotActive =
            Error.Unauthorized("Auth.AccountNotActive", "Your account is not active. Please contact support.");

        public static readonly Error EmailAlreadyExists =
            Error.Conflict("Auth.EmailAlreadyExists", "Email already exists.");

        public static readonly Error PasswordMismatch =
            Error.Validation("Auth.PasswordMismatch", "Passwords do not match.");

        public static readonly Error UserNotFound =
            Error.NotFound("Auth.UserNotFound", "User not found.");

        public static readonly Error UserNotAuthenticated =
            Error.Unauthorized("Auth.NotAuthenticated", "User not authenticated.");

        public static readonly Error InvalidToken =
            Error.Unauthorized("Auth.InvalidToken", "Invalid or expired token.");

        public static readonly Error RegistrationFailed =
            Error.Failure("Auth.RegistrationFailed", "User registration failed.");

        public static readonly Error ChangePasswordFailed =
            Error.Failure("Auth.ChangePasswordFailed", "Failed to change password.");

        public static readonly Error ResetPasswordFailed =
            Error.Failure("Auth.ResetPasswordFailed", "Failed to reset password.");

        public static readonly Error RefreshTokenFailed =
            Error.Failure("Auth.RefreshTokenFailed", "An error occurred while refreshing token.");
    }

    public static class Payment
    {
        public static readonly Error IdempotencyKeyAlreadyExists =
            Error.Conflict("Payment.IdempotencyKeyAlreadyExists", "A payment with the same idempotency key already exists.");

        public static readonly Error PaymentNotFound =
            Error.NotFound("Payment.PaymentNotFound", "Payment not found.");

        public static readonly Error PaymentAlreadyProcessed =
            Error.Validation("Payment.PaymentAlreadyProcessed", "Payment has already been processed.");
    }

    public static class Dish
    {
        public static readonly Error DishNotFound =
            Error.NotFound("Dish.DishNotFound", "Dish not found.");

        public static readonly Error DishUnavailable =
            Error.Validation("Dish.DishUnavailable", "Dish is currently unavailable.");
    }

    public static class Address
    {
        public static readonly Error AddressNotFound =
            Error.NotFound("Address.AddressNotFound", "Address not found.");
    }

    public static class Order
    {
        public static readonly Error OrderNotFound =
            Error.NotFound("Order.OrderNotFound", "Order not found.");
        public static readonly Error OrderAlreadyProcessed =
            Error.Validation("Order.OrderAlreadyProcessed", "Order has already been processed.");
        public static readonly Error InvalidOrderStatus =
            Error.Validation("Order.InvalidOrderStatus", "Invalid order status for this operation.");
        public static readonly Error DeliveryAddressNotProvided =
            Error.Validation("Order.DeliveryAddressNotProvided", "Delivery address is required.");
        public static readonly Error OrderAlreadyCancelled =
            Error.Validation("Order.OrderAlreadyCancelled", "This order has already been cancelled.");
        public static readonly Error CancellationNotAllowed =
            Error.Validation("Order.CancellationNotAllowed", "Cancellation is not allowed once preparation has started.");
        public static readonly Error UnauthorizedCancellation =
            Error.Forbidden("Order.UnauthorizedCancellation", "You are not authorized to cancel this order.");
        public static readonly Error InvalidCancellationReason =
            Error.Validation("Order.InvalidCancellationReason", "The selected cancellation reason does not match your role.");
    }
}
