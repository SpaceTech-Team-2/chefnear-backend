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
}
