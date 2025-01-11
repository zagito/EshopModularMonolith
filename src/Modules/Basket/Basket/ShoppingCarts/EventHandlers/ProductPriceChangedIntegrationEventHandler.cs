using Basket.ShoppingCarts.Features.UpdateItemPriceInShoppingCart;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedMessaging.IntegrationEvents;

namespace Basket.ShoppingCarts.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler(ILogger<ProductPriceChangedIntegrationEventHandler> logger,
    ISender sender) : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Integration event handled: {integrationEvent}", context.Message.GetType().Name);

        await sender.Send(new UpdateItemPriceInShoppingCartCommand(context.Message.ProductId, context.Message.Price));
    }
}
