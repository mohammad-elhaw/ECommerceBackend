using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Basket.Data.Processors;
public class OutboxProcessor (IServiceProvider sp, ILogger<OutboxProcessor> logger, IBus bus)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                var outboxMessages = await context.OutboxMessages
                    .Where(m => !m.ProcessedOn.HasValue)
                    .ToListAsync(stoppingToken);

                foreach(var message in outboxMessages)
                {
                    var eventType = Type.GetType(message.Type);
                    if(eventType is null)
                    {
                        logger.LogWarning("Could not resolve type {Type} for outbox message {MessageId}", message.Type, message.Id);
                        continue;
                    }

                    var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                    if(eventMessage is null)
                    {
                        logger.LogWarning("Could not deserialize content for outbox message {MessageId}", message.Id);
                        continue;
                    }
                    await bus.Publish(eventMessage, stoppingToken);
                    message.ProcessedOn = DateTime.UtcNow;
                    logger.LogInformation("Successfully Processed outbox message {MessageId} of type {Type}", message.Id, message.Type);
                }
                await context.SaveChangesAsync(stoppingToken);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error processing outbox messages");
            }
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        }
    }
}
