using ChefNear.Application.Features.Auth.Commands.ChangePassword;
using ChefNear.Application.Features.Auth.Commands.ConfirmEmail;
using ChefNear.Application.Features.Auth.Commands.ForgetPassword;
using ChefNear.Application.Features.Auth.Commands.Login;
using ChefNear.Application.Features.Auth.Commands.Logout;
using ChefNear.Application.Features.Auth.Commands.RefreshToken;
using ChefNear.Application.Features.Auth.Commands.Register;
using ChefNear.Application.Features.Auth.Commands.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefNear.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await _mediator.Send(command);

        if (response.IsFailure)
        {
            return Unauthorized(response.Error);
        }

        return Ok(response.Value);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand? command = null)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();


        var refreshToken = command?.RefreshToken
            ?? Request.Headers["RefreshToken"].FirstOrDefault();


        var logoutCommand = new LogoutCommand
        {
            UserId = userId,
            RefreshToken = refreshToken
        };


        var response = await _mediator.Send(logoutCommand);


        if (response.IsFailure)
            return BadRequest(response);


        return Ok(response);
    }


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command)
    {
        var response = await _mediator.Send(command);


        if (response.IsFailure)
            return Unauthorized(response);


        return Ok(response);
    }


    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string userId,
        [FromQuery] string token)
    {
        var command = new ConfirmEmailCommand
        {
            UserId = userId,
            Token = token
        };


        var response = await _mediator.Send(command);


        if (response.IsFailure)
            return BadRequest(response);


        return Ok(response);
    }


    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgetPasswordComand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }


    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordCommand command)
    {
        var response = await _mediator.Send(command);


        if (response.IsFailure)
            return BadRequest(response);


        return Ok(response);
    }


    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordComand command)
    {
        var response = await _mediator.Send(command);


        if (response.IsFailure)
            return BadRequest(response);


        return Ok(response);
    }
}