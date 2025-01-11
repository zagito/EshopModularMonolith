using MassTransit;
using SharedMessaging.IntegrationEvents;

namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEvent> logger, IPublishEndpoint publishEndpoint)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Product {ProductId} price is changed to {Price}", notification.Product.Id, notification.Product.Price);

        var integrationEvent = new ProductPriceChangedIntegrationEvent 
        {
            ProductId = notification.Product.Id,
            ProductName = notification.Product.Name,
            Price = notification.Product.Price,
            Category = notification.Product.Category,
            Description = notification.Product.Description,
            ImageFile = notification.Product.ImageFile,
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
