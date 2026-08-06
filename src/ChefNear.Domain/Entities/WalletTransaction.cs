using ChefNear.Domain.Common;

namespace ChefNear.Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; } = default!;

        public decimal Amount { get; set; }
        public decimal AmountAfter { get; set; }

        public WalletTransactionType Type { get; set; }

        public string Description { get; set; } = string.Empty;

        public Guid? OrderId { get; set; }  
        public Order? Order { get; set; }
    }
}