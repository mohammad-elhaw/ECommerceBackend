namespace Shared.Messaging.Events;
public record ProductPriceChangedIntegrationEvent : IntegrationEvent
{
    public Guid ProductId { get; init; }
    public string UserName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public List<string> Categories { get; init; } = [];
    public string Description { get; init; } = null!;
    public string ImageFile { get; init; } = null!;
    public decimal Price { get; init; }
}
