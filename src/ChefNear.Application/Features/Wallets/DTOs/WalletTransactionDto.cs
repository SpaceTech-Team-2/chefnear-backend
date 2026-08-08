using ChefNear.Domain.Entities;

namespace ChefNear.Application.Features.Wallets.DTOs;

public class WalletTransactionDto 
{
    public decimal Amount { get; set; }
    public WalletTransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? OrderId { get; set; }
}
