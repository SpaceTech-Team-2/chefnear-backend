#nullable enable

using System.Linq.Expressions;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Repositories;
using ChefNear.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChefNear.Infrastructure.Repositories;

public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
    where TEntity : BaseEntity<TId>
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id! }, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }
}

public class GenericRepository<TEntity> : GenericRepository<TEntity, int>, IGenericRepository<TEntity>
    where TEntity : BaseEntity<int>
{
    public GenericRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
