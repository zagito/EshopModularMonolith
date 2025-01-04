namespace Catalog.Products.EventHandlers;

public class ProdictPriceChangedEventHandler(ILogger<ProductPriceChangedEvent> logger) : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Product {ProductId} price is changed to {Price}", notification.Product.Id, notification.Product.Price);
        return Task.CompletedTask;
    }
}
