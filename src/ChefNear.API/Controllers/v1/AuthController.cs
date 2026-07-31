using Asp.Versioning;
using ChefNear.API.Controllers;
using ChefNear.Application.Features.Auth.Commands.changePassword;
using ChefNear.Application.Features.Auth.Commands.ConfirmEmail;
using ChefNear.Application.Features.Auth.Commands.ForgetPassword;
using ChefNear.Application.Features.Auth.Commands.Login;
using ChefNear.Application.Features.Auth.Commands.Logout;
using ChefNear.Application.Features.Auth.Commands.RefreshToken;
using ChefNear.Application.Features.Auth.Commands.Register;
using ChefNear.Application.Features.Auth.Commands.ResetPassword;
using ChefNear.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefNear.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseApiController
{
    // 1. Register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Registration successful. Please check your email to confirm your account.");
    }

    // 2. Login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Login successful.");
    }

    // 3. Logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand? command = null)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(ApiResponse.FailureResponse("User not authenticated.", statusCode: 401));
        }

        var refreshToken = command?.RefreshToken ?? Request.Headers["RefreshToken"].FirstOrDefault();

        var logoutCommand = new LogoutCommand
        {
            UserId = userId,
            RefreshToken = refreshToken
        };

        var result = await Mediator.Send(logoutCommand);
        return HandleResult(result, "Logged out successfully.");
    }

    // 4. Refresh Token
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Token refreshed successfully.");
    }

    // 5. Confirm Email
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var command = new ConfirmEmailCommand
        {
            UserId = userId,
            Token = token
        };

        var result = await Mediator.Send(command);
        return HandleResult(result, "Email confirmed successfully.");
    }

    // 6. Forgot Password
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordComand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "If an account exists, a reset link has been sent.");
    }

    // 7. Reset Password
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Password reset successfully.");
    }

    // 8. Change Password (Authenticated)
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordComand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Password changed successfully.");
    }
}
