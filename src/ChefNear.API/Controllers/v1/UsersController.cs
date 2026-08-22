using Asp.Versioning;
using ChefNear.Application.Features.Auth.Commands.Profile.Commands.DeleteProfileImage;
using ChefNear.Application.Features.Auth.Commands.Profile.Commands.UploadProfileImage;
using ChefNear.Application.Features.Auth.Commands.Profile.DTOs;
using ChefNear.Application.Features.Auth.Queries.Profile.GetMyProfile;
using ChefNear.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/profile")]
[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
public class ProfileController : BaseApiController
{
    [HttpPost("image")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var userId = Guid.Parse(GetUser().Id);

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var command = new UploadProfileImageCommand(
            userId,
            ms.ToArray(),
            file.FileName);

        var result = await Mediator.Send(command);
        return HandleResult(result, "Profile image uploaded successfully.");
    }

    [HttpDelete("image")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProfileImage()
    {
        var userId = GetUser().Id;

        var command = new DeleteProfileImageCommand(userId);

        var result = await Mediator.Send(command);
        return HandleResult(result, "Profile image deleted successfully.");
    }

    [HttpGet("me")]
    [ProducesResponseType<ApiResponse<ProfileDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetUser().Id;

        var result = await Mediator.Send(new GetMyProfileQuery(userId));
        return HandleResult(result);
    }
}