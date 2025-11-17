namespace Basket.Basket.Dtos;
public record BasketCheckoutDto
{
    public string UserName { get; init; } = default!;
    public Guid CustomerId { get; init; }
    public decimal TotalAmount { get; init; }


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
