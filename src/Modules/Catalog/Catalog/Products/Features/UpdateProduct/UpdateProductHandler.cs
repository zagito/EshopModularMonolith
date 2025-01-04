namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductComand(
    Guid Id,
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile)
    : ICommand;

public class UpdateProductHandler(CatalogDbContext dbContext) : ICommandHandler<UpdateProductComand>
{
    public async Task<Result> Handle(UpdateProductComand request, CancellationToken cancellationToken)
    {
        Product? product = await dbContext.Products.FindAsync([request.Id], cancellationToken : cancellationToken);

        if (product is null)
            return Error.ProductNotFound;

        product.Update(
            request.Name,
            request.Category,
            request.Description,
            request.ImageFile,
            request.Price);

        //dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
