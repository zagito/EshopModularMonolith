namespace Shared.DDD;

public interface IAggregate<TKey> : IAggregate, IEntity<TKey>
{
}

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}
