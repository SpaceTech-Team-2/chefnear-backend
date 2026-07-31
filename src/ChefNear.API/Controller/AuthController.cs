using ChefNear.Application.Features.Auth.Commands.changePassword;
using ChefNear.Application.Features.Auth.Commands.ConfirmEmail;
using ChefNear.Application.Features.Auth.Commands.ForgetPassword;
using ChefNear.Application.Features.Auth.Commands.Login;
using ChefNear.Application.Features.Auth.Commands.Logout;
using ChefNear.Application.Features.Auth.Commands.RefreshToken;
using ChefNear.Application.Features.Auth.Commands.Register;
using ChefNear.Application.Features.Auth.Commands.ResetPassword;
using ChefNear.Application.Responce;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefNear.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }



        // 1. Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        // 2. Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return Unauthorized(response);
            return Ok(response);
        }

        // 3. Logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand? command = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new BaseCommandResponse
                {
                    Success = false,
                    Message = "User not authenticated"
                });
            }

            var refreshToken = command?.RefreshToken ?? Request.Headers["RefreshToken"].FirstOrDefault();

            var logoutCommand = new LogoutCommand
            {
                UserId = userId,
                RefreshToken = refreshToken
            };

            var response = await _mediator.Send(logoutCommand);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // 4. Refresh Token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return Unauthorized(response);
            return Ok(response);
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

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        // 6. Forgot Password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordComand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // 7. Reset Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        // 8. Change Password (Authenticated)
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordComand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
    }


}

