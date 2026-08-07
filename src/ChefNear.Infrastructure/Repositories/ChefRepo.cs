using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;

namespace ChefNear.Infrastructure.Repositories
{
    public class ChefRepo : GenericRepository<Chef>, IChefRepo
    {
        public ChefRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
        }
    }
}
