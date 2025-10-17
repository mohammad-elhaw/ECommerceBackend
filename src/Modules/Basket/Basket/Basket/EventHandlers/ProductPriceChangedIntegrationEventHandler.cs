using Basket.Basket.Features.UpdateItemPriceBasket;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers;
public class ProductPriceChangedIntegrationEventHandler
    (ILogger<ProductPriceChangedIntegrationEventHandler> logger,
    ISender sender)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event handled : {IntegrationEvent}", 
            context.Message.GetType().Name);
        var result = await sender.Send(new UpdateItemPriceInBasketCommand(
            context.Message.ProductId, 
            context.Message.Price));

        if(!result.Success)
            logger.LogError("Failed to update item price in basket for ProductId: {ProductId}", 
                context.Message.ProductId);

        logger.LogInformation("Successfully updated item price in basket for ProductId: {ProductId}", 
            context.Message.ProductId);
    }
}
