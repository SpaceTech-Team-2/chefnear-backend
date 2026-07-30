namespace ChefNear.Domain.Entities;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;
    public bool IsDeleted { get; set; } = false;
}

public abstract class BaseEntity : BaseEntity<int>
{
}

public abstract class BaseAuditableEntity<TId> : BaseEntity<TId>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

public abstract class BaseAuditableEntity : BaseAuditableEntity<int>
{
}
