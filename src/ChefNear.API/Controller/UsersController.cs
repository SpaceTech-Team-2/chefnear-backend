using ChefNear.Application.Features.Auth.Commands.Profile.Commands.DeleteProfileImage;
using ChefNear.Application.Features.Auth.Commands.Profile.Commands.UploadProfileImage;
using ChefNear.Application.Features.Auth.Queries.Profile.GetMyProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefNear.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var command = new UploadProfileImageCommand
        {
            UserId = Guid.Parse(userId),         
            FileBytes = ms.ToArray(),
            FileName = file.FileName
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { message = "Profile image uploaded successfully." });
    }
    [HttpDelete("image")]
    public async Task<IActionResult> DeleteProfileImage()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var command = new DeleteProfileImageCommand
        {
            UserId = userId
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { message = "Profile image deleted successfully." });
    }
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var query = new GetMyProfileQuery { UserId = userId };
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}