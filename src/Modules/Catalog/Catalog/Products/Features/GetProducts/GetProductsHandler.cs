using Catalog.Products.Dtos;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery() : IQuery<IEnumerable<ProductDto>>;

public class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Category,
                p.Description,
                p.ImageFile,
                p.Price))
            .ToListAsync(cancellationToken);
    }
}
