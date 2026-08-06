using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;

namespace ChefNear.Infrastructure.Repositories
{
    public class ClientRepo : GenericRepository<Client>, IClientRepo
    {
        public ClientRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
        }
    }
}
