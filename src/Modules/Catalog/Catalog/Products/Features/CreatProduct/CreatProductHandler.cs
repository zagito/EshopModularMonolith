namespace Catalog.Products.Features.CreatProduct;

public record CreatProductComand(
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile)
    : ICommand<Guid>;

public class CreatProductHandler(CatalogDbContext dbContext) : ICommandHandler<CreatProductComand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatProductComand comand, CancellationToken cancellationToken)
    {
        Product product = Product.Create(
            comand.Name,
            comand.Category,
            comand.Description,
            comand.ImageFile,
            comand.Price);
        
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
