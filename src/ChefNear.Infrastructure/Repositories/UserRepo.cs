using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Domain.Entities;
using ChefNear.Infrastructure.Persistence;

namespace ChefNear.Infrastructure.Repositories
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private readonly ChefNearDbContext _dbContext;

        public UserRepo(ChefNearDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _dbContext.Users.FindAsync(id);
        }
    }
}