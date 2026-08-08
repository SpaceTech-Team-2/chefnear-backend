#nullable enable
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ChefNear.Application.Common.Persistence.Interfaces;
using ChefNear.Infrastructure.Persistence;

namespace ChefNear.Infrastructure.Repositories;


    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ChefNearDbContext _dbContext;

        public GenericRepository(ChefNearDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public virtual async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public virtual Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

    public virtual Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, params string[] includes)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }
}
