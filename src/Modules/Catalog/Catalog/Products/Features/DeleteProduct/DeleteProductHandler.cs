namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductComand(Guid Id) : ICommand;

internal class DeleteProductHandler(CatalogDbContext dbContext) : ICommandHandler<DeleteProductComand>
{
    public async Task<Result> Handle(DeleteProductComand request, CancellationToken cancellationToken)
    {
        Product? product = await dbContext.Products.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (product is null)
            return Error.ProductNotFound;

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
