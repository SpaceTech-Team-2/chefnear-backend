using System.Linq.Expressions;
using ChefNear.Domain.Entities;

namespace ChefNear.Domain.Repositories;

public interface IGenericRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}

public interface IGenericRepository<TEntity> : IGenericRepository<TEntity, int> where TEntity : BaseEntity<int>
{
}
