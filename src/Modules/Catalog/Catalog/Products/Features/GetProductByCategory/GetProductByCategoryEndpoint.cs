namespace Catalog.Products.Features.GetProductByCategory;

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ProductsRoot}/category/{{category}}", GetProductByCategory)
            .WithTags(ProductsTag)
            .WithName(nameof(GetProductByCategory));
    }

    private static async Task<EndpointResult<IEnumerable<ProductDto>>> GetProductByCategory(string category, ISender sender)
    {
        GetProductByCategoryQuery query = new(category);
        return await sender.Send(query);
    }
}