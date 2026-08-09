using Asp.Versioning;
using ChefNear.Application.Features.Address.Queries.GetAddressById;
using ChefNear.Application.Features.Addresses.Commands;
using ChefNear.Application.Features.Addresses.Queries;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Application.Features.Addresses.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChefNear.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
public class AddressesController : BaseApiController
{


    [HttpGet("my")]
    public async Task<IActionResult> GetMyAddresses()
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await Mediator.Send(
            new GetUserAddressesQuery
            {
                UserId = Guid.Parse(userId)
            });

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
      [FromBody] CreateAddressCommand command)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.UserId = Guid.Parse(userId);

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Address created successfully.");
    }
    [HttpPut("{addressId:guid}")]
    public async Task<IActionResult> Update(
         Guid addressId,
         [FromBody] UpdateAddressCommand command)
    {
        if (addressId != command.AddressId)
            return BadRequest("Route addressId does not match request body.");

        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        command.UserId = Guid.Parse(userId);

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Address updated successfully.");
    }

    [HttpDelete("{addressId:guid}")]
    public async Task<IActionResult> Delete(Guid addressId)
    {
        var userId = GetUser().Id;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var command = new DeleteAddressCommand
        {
            AddressId = addressId,
            UserId = Guid.Parse(userId)
        };

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Address deleted successfully.");
    }
    [HttpGet("{addressId:guid}")]
    public async Task<IActionResult> GetById(Guid addressId)
    {
        var result = await Mediator.Send(
            new GetAddressByIdQuery
            {
                AddressId = addressId
            });

        return HandleResult(result);
    }
}
