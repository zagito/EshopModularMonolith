namespace Catalog.Products.Features.GetProductById;

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", GetProductById)
            .WithTags(ProductsRoot)
            .WithName(nameof(GetProductById));
    }

    private static async Task<EndpointResult<ProductDto>> GetProductById(Guid id, ISender sender)
    {
        GetProductByIdQuery query = new(id);
        return await sender.Send(query);
    }
}
