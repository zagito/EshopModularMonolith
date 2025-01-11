namespace CatalogContracts.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
