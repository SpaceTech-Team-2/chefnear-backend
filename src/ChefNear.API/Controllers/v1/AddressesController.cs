using Asp.Versioning;
using ChefNear.Application.Features.Address.DTOs;
using ChefNear.Application.Features.Address.Queries.GetAddressById;
using ChefNear.Application.Features.Addresses.Commands;
using ChefNear.Application.Features.Addresses.Queries;
using ChefNear.Shared.Responses;
using HomeChefMarketplace.Application.Features.Addresses.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
[Authorize]
public class AddressesController : BaseApiController
{
    [HttpGet("my")]
    [ProducesResponseType<ApiResponse<List<GetAddressDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyAddresses()
    {
        var userId = Guid.Parse(GetUser().Id);

        var result = await Mediator.Send(
            new GetUserAddressesQuery(userId));

        return HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType<ApiResponse<Guid>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(
      [FromBody] CreateAddressRequest request)
    {
        var userId = Guid.Parse(GetUser().Id);

        var command = new CreateAddressCommand(
            userId,
            request.Label,
            request.City,
            request.Details,
            request.Latitude,
            request.Longitude,
            request.IsDefault);

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Address created successfully.");
    }

    [HttpPut("{addressId:guid}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
         Guid addressId,
         [FromBody] UpdateAddressRequest request)
    {
        var userId = Guid.Parse(GetUser().Id);

        var command = new UpdateAddressCommand(
            addressId,
            userId,
            request.Label,
            request.City,
            request.Details,
            request.Latitude,
            request.Longitude,
            request.IsDefault);

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Address updated successfully.");
    }

    [HttpDelete("{addressId:guid}")]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid addressId)
    {
        var userId = Guid.Parse(GetUser().Id);

        var command = new DeleteAddressCommand(addressId, userId);

        var result = await Mediator.Send(command);

        return HandleResult(
            result,
            "Address deleted successfully.");
    }

    [HttpGet("{addressId:guid}")]
    [ProducesResponseType<ApiResponse<GetAddressDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid addressId)
    {
        var result = await Mediator.Send(
            new GetAddressByIdQuery(addressId));

        return HandleResult(result);
    }
}
