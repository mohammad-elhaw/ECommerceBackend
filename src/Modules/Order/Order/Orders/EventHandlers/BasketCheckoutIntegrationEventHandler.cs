using MassTransit;
using Microsoft.Extensions.Logging;
using Order.Orders.Feature.CreateOrder;
using Shared.Messaging.Events;

namespace Order.Orders.EventHandlers;
public class BasketCheckoutIntegrationEventHandler
    (ILogger<BasketCheckoutIntegrationEvent> logger, ISender sender)
    : IConsumer<BasketCheckoutIntegrationEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        logger.LogInformation("Integration event handled{IntegrationEvent}", context.Message.GetType().Name);
        var createdOrderCommand = MapToCreateOrderCommand(context.Message);
        await sender.Send(createdOrderCommand);
    }

    private static CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntegrationEvent integrationEvent)
    {
        Guid orderId = Guid.NewGuid();
        return new CreateOrderCommand(new OrderDto
        (
            orderId,
            integrationEvent.CustomerId,
            integrationEvent.UserName,
            new AddressDto
            (
                integrationEvent.EmailAddress,
                integrationEvent.AddressLine,
                integrationEvent.Country,
                integrationEvent.State,
                integrationEvent.ZipCode
            ),
            new AddressDto
            (
                integrationEvent.EmailAddress,
                integrationEvent.AddressLine,
                integrationEvent.Country,
                integrationEvent.State,
                integrationEvent.ZipCode
            ),
            new PaymentDto
            (
                integrationEvent.CardName,
                integrationEvent.CardNumber,
                integrationEvent.Expiration,
                integrationEvent.PaymentMethod,
                integrationEvent.Cvv
            ),
            integrationEvent.Items.Select(item => new OrderItemDto
            (

                item.OrderId,
                item.ProductId,
                item.Quantity,
                item.Price
            )).ToList()
        ));
    }
}
