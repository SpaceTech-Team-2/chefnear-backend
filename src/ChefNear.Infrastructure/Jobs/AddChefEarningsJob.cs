using ChefNear.Application.Common.Jobs;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
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

        try
        {
            // update Payment Status
            payment.Release();

            // Add earnings to Chef Wallet
            var transaction = wallet.AddEarnings(amountToAdd, payment.OrderId);
            await unitOfWork.Transactions.AddAsync(transaction);

            await unitOfWork.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException ex)
        {
            logger.LogWarning(ex,
                "Concurrency conflict while adding earnings to Chef {ChefId} for Payment {PaymentId}.",
                chefId,
                paymentId);

            foreach (var entry in ex.Entries)
            {
                logger.LogWarning(
                    "Concurrency conflict on entity {EntityType}, key {Key}, State: {State}",
                    entry.Metadata.ClrType.Name,
                    entry.Properties
                        .Where(p => p.Metadata.IsPrimaryKey())
                        .Select(p => p.CurrentValue)
                        .FirstOrDefault(),
                    entry.State);
            }

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
