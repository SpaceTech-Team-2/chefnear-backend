using Asp.Versioning;
using ChefNear.API.Controllers;
using ChefNear.Application.Features.Auth.Commands.changePassword;
using ChefNear.Application.Features.Auth.Commands.ChangePassword;
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

namespace ChefNear.API.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuthController : BaseApiController
{
    // 1. Register
    [HttpPost("register")]
    [ProducesResponseType<ApiResponse<RegisterResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Registration successful. Please check your email to confirm your account.");
    }

    // 2. Login
    [HttpPost("login")]
    [ProducesResponseType<ApiResponse<LoginResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Login successful.");
    }

    // 3. Logout
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType<ApiResponse<LogoutResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest? request = null)
    {
        var userId = GetUser().Id;

        var refreshToken = request?.RefreshToken ?? Request.Headers["RefreshToken"].FirstOrDefault();

        var logoutCommand = new LogoutCommand(userId, refreshToken);

        var result = await Mediator.Send(logoutCommand);
        return HandleResult(result, "Logged out successfully.");
    }

    // 4. Refresh Token
    [HttpPost("refresh-token")]
    [ProducesResponseType<ApiResponse<RefreshTokenResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Token refreshed successfully.");
    }

    // 5. Confirm Email
    [HttpGet("confirm-email")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var command = new ConfirmEmailCommand(userId, token);

        var result = await Mediator.Send(command);
        return HandleResult(result, "Email confirmed successfully.");
    }

    // 6. Forgot Password
    [HttpPost("forgot-password")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordComand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "If an account exists, a reset link has been sent.");
    }

    // 7. Reset Password
    [HttpPost("reset-password")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Password reset successfully.");
    }

    // 8. Change Password (Authenticated)
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType<ApiResponse<ChangePasswordResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordComand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Password changed successfully.");
    }
}
