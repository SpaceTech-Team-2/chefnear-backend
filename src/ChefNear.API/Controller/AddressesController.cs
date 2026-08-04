using ChefNear.Application.Features.Address.Queries.GetAddressById;
using ChefNear.Application.Features.Addresses.Commands;
using ChefNear.Application.Features.Addresses.Queries;
using ChefNear.Shared.ResultPattern;
using HomeChefMarketplace.Application.Features.Addresses.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controller;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AddressesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetUserAddresses(Guid userId)
    {
        var result = await _mediator.Send(new GetUserAddressesQuery
        {
            UserId = userId
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAddressCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error.Description);

        return Ok(new
        {
            message = "Address created successfully.",
            addressId = result.Value
        });
    }

    [HttpPut("{addressId:guid}")]
    public async Task<IActionResult> Update(Guid addressId, [FromBody] UpdateAddressCommand command)
    {
        if (addressId != command.AddressId)
            return BadRequest("Route addressId does not match request body.");

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error.Description);

        return Ok(new { message = "Address Updated successfully." });
    }

    [HttpDelete("{addressId:guid}")]
    public async Task<IActionResult> Delete(Guid addressId, [FromQuery] Guid userId)
    {
        var command = new DeleteAddressCommand
        {
            AddressId = addressId,
            UserId = userId
        };

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error.Description);

        return Ok(new { message = "Address Updated successfully." });
    }
    [HttpGet("{addressId:guid}")]
    public async Task<IActionResult> GetById(Guid addressId)
    {
        var res = await _mediator.Send(new GetAddressByIdQuery { AddressId = addressId });
        if (res.IsFailure)
            return NotFound(res.Error);

        return Ok(res.Value);
    }

}
