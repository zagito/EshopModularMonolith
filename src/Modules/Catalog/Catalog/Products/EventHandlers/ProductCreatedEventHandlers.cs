namespace Catalog.Products.EventHandlers;

public class ProductCreatedEventHandlers(ILogger<ProductCreatedEventHandlers> logger) : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Product {ProductId} is created", notification.Product.Id);
        return Task.CompletedTask;
    }
}
