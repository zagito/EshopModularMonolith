using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", GetAllProducts)
            .WithTags("Products")
            .WithName("GetAllProducts");

        static async Task<EndpointResult<PaginationResult<ProductDto>>> GetAllProducts
            ([AsParameters] PaginationRequest request, ISender sender)
        {
            GetProductsQuery query = new(request);
            return await sender.Send(query);
        }
    }
}
