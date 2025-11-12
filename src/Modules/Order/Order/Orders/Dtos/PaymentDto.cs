namespace Order.Orders.Dtos;
public record PaymentDto
(
    string CardName,
    string CardNumber,
    string Expiration,
    int PaymentMethod,
    string Cvv
);