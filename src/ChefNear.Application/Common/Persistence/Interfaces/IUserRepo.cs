using ChefNear.Domain.Entities;

namespace ChefNear.Application.Common.Persistence.Interfaces
{
    public interface IUserRepo : IGenericRepository<User>
    {
        Task<User?> GetByIdAsync(string id);
    }
}