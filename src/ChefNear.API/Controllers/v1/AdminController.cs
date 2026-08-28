using Asp.Versioning;
using ChefNear.Application.Features.Admin.Command.AdminDeleteReview;
using ChefNear.Application.Features.Admin.Command.DeleteUser;
using ChefNear.Application.Features.Admin.Queries.GetAllReviews;
using ChefNear.Application.Features.Admin.Queries.GetAllUsersQuery;
using ChefNear.Application.Features.Admin.Queries.GetMonthlyReportQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[Authorize(Roles = "Admin")]
public class AdminController : BaseApiController
{
    // ===== Users =====
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result, "Admin created successfully.");
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var result = await Mediator.Send(new DeleteUserCommand(userId));
        return HandleResult(result, "User deleted successfully.");
    }

    // ===== Reviews =====
    [HttpGet("reviews")]
    public async Task<IActionResult> GetAllReviews([FromQuery] GetAllReviewsQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

  

    [HttpDelete("reviews/{reviewId:guid}")]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        var result = await Mediator.Send(new AdminDeleteReviewCommand(reviewId));
        return HandleResult(result, "Review deleted successfully.");
    }

    // ===== Reports =====
    [HttpGet("reports/monthly")]
    public async Task<IActionResult> GetMonthlyReport()
    {
        var result = await Mediator.Send(new GetMonthlyReportQuery());
        return HandleResult(result);
    }
}