using AutoMapper;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Errors;
using ChefNear.Shared.ResultPattern;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChefNear.Application.Features.Wallets.Commands.Withdraw;

public class WalletWithdrawCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<WalletWithdrawCommandHandler> logger,
    IMapper mapper)
    : IRequestHandler<WalletWithdrawCommand, Result<WalletWithdrawalResponse>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ILogger<WalletWithdrawCommandHandler> logger = logger;
    private readonly IMapper mapper = mapper;

    public async Task<Result<WalletWithdrawalResponse>> Handle(WalletWithdrawCommand request, CancellationToken cancellationToken)
    {
        var wallet = await unitOfWork.Wallets
            .GetAsync(w => w.ChefId == request.Chef.Id, nameof(Wallet.Transactions));

        if (wallet == null)
            return DomainErrors.Wallet.WalletNotFound;

        try
        {
            var transaction = wallet.Withdraw(request.Amount);
            await unitOfWork.Transactions.AddAsync(transaction);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<WalletWithdrawalResponse>(transaction);

            return Result.Success(response);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogWarning(
                ex,
                "Concurrency conflict while withdrawing {Amount} from wallet {WalletId}",
                request.Amount,
                wallet.Id);

            return Error.Conflict("Wallet.WalletConcurrencyConflict", "The wallet was modified by another transaction. Please try again.");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(
                ex,
                "Invalid withdrawal of {Amount} from wallet {WalletId}",
                request.Amount,
                wallet.Id);

            return Error.Validation("Wallet.InvalidWithdrawal", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(
                ex,
                "Invalid wallet operation while withdrawing {Amount} from wallet {WalletId}",
                request.Amount,
                wallet.Id);

            return Error.Validation("Wallet.InvalidOperation", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error while withdrawing {Amount} from wallet {WalletId}",
                request.Amount,
                wallet.Id);

            return Error.Failure("Wallet.WithdrawalFailed", "An unexpected error occurred while processing the withdrawal.");
        }
    }
}
