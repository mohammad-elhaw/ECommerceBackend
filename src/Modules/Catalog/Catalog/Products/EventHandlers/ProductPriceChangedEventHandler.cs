﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Products.EventHandlers;
public class ProductPriceChangedEventHandler (ILogger<ProductPriceChangedEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Implement integration event to notify other bounded contexts
        logger.LogInformation("Domain Event handled : {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
