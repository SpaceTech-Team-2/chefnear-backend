using ChefNear.Domain.Entities;

namespace ChefNear.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : BaseEntity<TId>;
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<int>;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
