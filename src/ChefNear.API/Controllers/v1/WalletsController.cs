using Asp.Versioning;
using ChefNear.Application.Features.Wallets.Commands.Withdraw;
using ChefNear.Application.Features.Wallets.DTOs;
using ChefNear.Application.Features.Wallets.Queries;
using ChefNear.Shared.Constants;
using ChefNear.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChefNear.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class WalletsController : BaseApiController
    {
        [HttpPut("withdraw")]
        [Authorize(Roles = UserRoles.Chef)]
        [ProducesResponseType<ApiResponse<WalletWithdrawalResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status409Conflict)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Withdraw(WalletWithdrawRequest request)
        {
            var command = new WalletWithdrawCommand(request.Amount, request.PayoutMethod, GetUser());

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("my-wallet")]
        [Authorize(Roles = UserRoles.Chef)]
        [ProducesResponseType<ApiResponse<GetMyWalletDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyWallet()
        {
            var query = new GetMyWalletQuery(GetUser());

            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
    }
}
