using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Infrastructure.Repositories
{
    public class WalletRepo : GenericRepository<Wallet>, IWalletRepo
    {
        public WalletRepo(ChefNearDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
