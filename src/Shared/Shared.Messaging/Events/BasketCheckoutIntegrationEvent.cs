namespace Shared.Messaging.Events;
public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public string UserName { get; init; } = default!;
    public Guid CustomerId { get; init; }
    public decimal TotalAmount { get; init; }

    // order items
    public List<BasketItemDto> Items { get; init; } = [];


    public string EmailAddress { get; init; } = default!;
    public string AddressLine { get; init; } = default!;
    public string Country { get; init; } = default!;
    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;

    public string CardName { get; init; } = default!;
    public string CardNumber { get; init; } = default!;
    public string Expiration { get; init; } = default!;
    public int PaymentMethod { get; init; }
    public string Cvv { get; init; } = default!;

}

public record BasketItemDto
{
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}
