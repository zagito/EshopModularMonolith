namespace Shared.DDD;

public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}