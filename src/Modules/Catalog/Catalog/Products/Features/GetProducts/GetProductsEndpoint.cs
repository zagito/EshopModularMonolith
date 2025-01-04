using Catalog.Products.Dtos;

namespace Catalog.Products.Features.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", GetAllProducts)
            .WithTags("Products")
            .WithName("GetAllProducts");

        static async Task<ModelResult<IEnumerable<ProductDto>>> GetAllProducts(ISender sender)
        {
            GetProductsQuery query = new();
            return await sender.Send(query);
        }
    }
}
