using ChefNear.Domain.Entities;

namespace ChefNear.Application.Features.Wallets.Commands.Withdraw;

public class WalletWithdrawalResponse
{
    public Guid TransactionId { get; set; } = default!;
    public WalletTransactionType Type { get; set; }
    public decimal Amoount { get; set; }
    public decimal AmoountAfter { get; set; }
    public DateTime CreatedAt { get; set; }
}
