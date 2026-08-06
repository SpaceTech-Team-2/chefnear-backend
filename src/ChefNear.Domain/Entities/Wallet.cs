using ChefNear.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace ChefNear.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public string ChefId { get; set; } = default!;
        public Chef Chef { get; set; } = default!;

        public decimal Balance { get; set; }
        public decimal TotalEarned { get; set; }
        public decimal TotalWithdrawn { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;

        public string Currency { get; set; } = "EGP";

        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
    }
}
