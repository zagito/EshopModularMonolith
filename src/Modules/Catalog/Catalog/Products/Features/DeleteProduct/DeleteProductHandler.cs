namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand;

internal class DeleteProductHandler(CatalogDbContext dbContext) : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await dbContext.Products.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

        if (product is null)
            return Error.ProductNotFound;

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
