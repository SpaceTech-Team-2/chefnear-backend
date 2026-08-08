using ChefNear.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChefNear.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public string ChefId { get; set; } = default!;
        public Chef Chef { get; set; } = default!;

        public decimal Balance { get; private set; }
        public decimal TotalEarned { get; private set; }
        public decimal TotalWithdrawn { get; private set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        public string Currency { get; set; } = "EGP";

        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();

        public void AddEarnings(decimal amount, Guid orderId, string? description = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Earnings amount must be greater than zero.", nameof(amount));

            Balance += amount;
            TotalEarned += amount;

            Transactions.Add(new WalletTransaction
            {
                Amount = amount,
                AmountAfter = Balance,
                Type = WalletTransactionType.OrderIncome,
                Description = description ?? $"Earnings from Order #{orderId}",
                OrderId = orderId
            });
        }

        public void Withdraw(decimal amount, string? description = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero.", nameof(amount));

            if (amount > Balance)
                throw new InvalidOperationException("Insufficient wallet balance for withdrawal.");

            Balance -= amount;
            TotalWithdrawn += amount;

            Transactions.Add(new WalletTransaction
            {
                Amount = amount,
                AmountAfter = Balance,
                Type = WalletTransactionType.Withdrawal,
                Description = description ?? "Withdrawal from wallet"
            });
        }

        public static Wallet Initialize(string chefId) => new Wallet
        {
            ChefId = chefId,
            Balance = 0,
            TotalEarned = 0,
            TotalWithdrawn = 0,
        };
    }
}
