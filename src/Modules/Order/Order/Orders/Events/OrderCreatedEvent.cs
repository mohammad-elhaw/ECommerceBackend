using Shared.DDD;

namespace Order.Orders.Events;
public record OrderCreatedEvent(Models.Order Order) : IDomainEvent;
