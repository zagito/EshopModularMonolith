namespace Catalog.Products.Features.GetProductByCategory;

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", GetProductByCategory)
            .WithTags("Products")
            .WithName("GetProductByCategory");

        static async Task<EndpointResult<IEnumerable<ProductDto>>> GetProductByCategory(string category, ISender sender)
        {
            GetProductByCategoryQuery query = new(category);
            return await sender.Send(query);
        }
    }
}