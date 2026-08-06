using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;

namespace ChefNear.Infrastructure.Repositories
{
    internal class WalletTransactionRepo : GenericRepository<WalletTransaction>, IWalletTransactionRepo
    {
        public WalletTransactionRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
        }
    }
}
