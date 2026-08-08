using ChefNear.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChefNear.Application.Features.Wallets.DTOs;

public class GetMyWalletDto
{
    public decimal Balance { get; private set; }
    public decimal TotalEarned { get; private set; }
    public decimal TotalWithdrawn { get; private set; }
    public string Currency { get; set; } = "EGP";

    public List<WalletTransactionDto> Transactions { get; set; }    
}
