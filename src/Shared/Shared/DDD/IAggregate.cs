namespace Shared.DDD;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}
