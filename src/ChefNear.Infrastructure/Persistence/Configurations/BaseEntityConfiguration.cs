using ChefNear.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChefNear.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TId>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        // Soft delete global query filter
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public abstract class BaseEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity, int>
    where TEntity : BaseEntity<int>
{
}
