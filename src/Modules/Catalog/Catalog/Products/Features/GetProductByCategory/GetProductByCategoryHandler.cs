using Catalog.Products.Dtos;

namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<IEnumerable<ProductDto>>;

public class GetProductByCategoryHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByCategoryQuery, IEnumerable<ProductDto>>
{
    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(request.Category))
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
