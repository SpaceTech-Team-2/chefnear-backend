using ChefNear.Application.Common.Jobs;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChefNear.Infrastructure.Jobs;

internal class AddChefEarningsJob(IUnitOfWork unitOfWork, ILogger<AddChefEarningsJob> logger) : IAddChefEarningsJob
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ILogger<AddChefEarningsJob> logger = logger;

    public async Task ExecuteAsync(Guid paymentId, string chefId)
    {
        var payment = await unitOfWork.Payments.GetByIdAsync(paymentId);

        if (payment == null)
        {
            logger.LogWarning("Attempting to add earnings to Chef: {ChefId} but Payment not founded.", chefId);
            return;
        }

        if(payment.Status != PaymentStatus.Held)
        {
            logger.LogWarning("Attempting to add earnings to Chef: {ChefId} but Payment.Status has invalid value {PaymentStatus}.",
                chefId,
                payment.Status);

            return;
        }

        var wallet = await unitOfWork.Wallets
            .GetAsync(w => w.ChefId ==  chefId, nameof(Wallet.Transactions));

        if (wallet == null)   
        {
            logger.LogWarning("Coudn't find the Wallet for Chef with Id: {ChefId}",
                chefId);

            return;
        }

        var amount = payment.Amount;
        var commissionPercent = 0.2M;

        var amountToAdd = Math.Max(Math.Round(amount - (amount * commissionPercent)), 0);    // 100 - (100 * 0.2) = 100 - 20 = 80

        var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            // update Payment Status
            payment.Status = PaymentStatus.Released;
            payment.ReleasedAt = DateTime.UtcNow;

            // Add earnings to Chef Wallet
            wallet.Balance += amountToAdd;
            wallet.TotalEarned += amountToAdd;

            var incomeTransaction = new WalletTransaction
            {
                Amount = amountToAdd,
                AmountAfter = wallet.Balance,
                Type = WalletTransactionType.OrderIncome,
                Description = $"Earnings from Order #{payment.OrderId}",
                OrderId = payment.OrderId,
            };

            // Add Wallet Transaction 
            wallet.Transactions.Add(incomeTransaction);

            await unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch(DbUpdateConcurrencyException ex)
        {
            logger.LogWarning(ex,
                "Concurrency conflict while adding earnings to Chef {ChefId} for Payment {PaymentId}.",
                chefId,
                paymentId);

            await transaction.RollbackAsync();

            throw;  // Hangfire will retry
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "An Expected Error Happen while add earnings to Chef: {ChefId} for Payment {PaymentId}.",
                chefId,
                paymentId);

            throw;  // Hangfire will retry
        }

        return;
    }
}
