using ChefNear.Application.Model;
using ChefNear.Domain.Enums;
using ChefNear.Shared.ResultPattern;
using MediatR;

namespace ChefNear.Application.Features.Wallets.Commands.Withdraw;

public record WalletWithdrawRequest(
    decimal Amount,
    PayoutMethods PayoutMethod
);

public record WalletWithdrawCommand(
    decimal Amount,
    PayoutMethods PayoutMethod,
    CurrentUser Chef
): IRequest<Result<WalletWithdrawalResponse>>;

