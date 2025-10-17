using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;
public class ProductPriceChangedEventHandler 
    (IBus bus, ILogger<ProductPriceChangedEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled : {DomainEvent}", notification.GetType().Name);

        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Description = notification.Product.Description,
            Price = notification.Product.Price,
            Categories = notification.Product.Categories,
            ImageFile = notification.Product.ImageFile,
        };

        await bus.Publish(integrationEvent, cancellationToken);
    }
}
