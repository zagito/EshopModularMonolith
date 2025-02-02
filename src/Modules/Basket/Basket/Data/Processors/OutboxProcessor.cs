using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Basket.Data.Processors;

public class OutboxProcessor (IServiceProvider serviceProvider, IBus bus, ILogger<OutboxProcessor> logger)
    : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) 
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                var outboxMessages = await dbContext.OutboxMessages
                    .Where(om => om.ProcessedOn == null)
                    .OrderBy(om => om.OccurredOn)
                    .Take(100)
                    .ToArrayAsync(stoppingToken);

                foreach (var outboxMessage in outboxMessages)
                {
                    var eventType = Type.GetType(outboxMessage.Type);
                    if (eventType is null)
                    {
                        logger.LogWarning("Event type {Type} not found", outboxMessage.Type);
                        continue;
                    }

                    var message = JsonSerializer.Deserialize(outboxMessage.Content, eventType);
                    if (message is null)
                    {
                        logger.LogWarning("Failed to deserialize message {Content}", outboxMessage.Content);
                        continue;
                    }

                    await bus.Publish(message, stoppingToken);
                    outboxMessage.MarkAsProcessed();
                }
                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred processing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
