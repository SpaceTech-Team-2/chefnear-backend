using System.Collections.Concurrent;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Repositories;
using ChefNear.Infrastructure.Persistence;

namespace ChefNear.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ConcurrentDictionary<string, object> _repositories = new();
    private bool _disposed;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : BaseEntity<TId>
    {
        var typeName = typeof(TEntity).Name + "_" + typeof(TId).Name;

        return (IGenericRepository<TEntity, TId>)_repositories.GetOrAdd(typeName, _ =>
            new GenericRepository<TEntity, TId>(_dbContext));
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<int>
    {
        var typeName = typeof(TEntity).Name + "_Int32";

        return (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeName, _ =>
            new GenericRepository<TEntity>(_dbContext));
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }
    }
}
