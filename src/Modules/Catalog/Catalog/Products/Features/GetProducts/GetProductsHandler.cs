using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest Request) : IQuery<PaginationResult<ProductDto>>;

public class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, PaginationResult<ProductDto>>
{
    public async Task<Result<PaginationResult<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pageIndex = request.Request.PageIndex;
        var pageSize = request.Request.PageSize;

        var count = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Category,
                p.Description,
                p.ImageFile,
                p.Price))
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginationResult<ProductDto>(pageIndex, pageSize, count, products);
    }
}
