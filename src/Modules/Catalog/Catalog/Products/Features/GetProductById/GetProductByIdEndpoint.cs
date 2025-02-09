namespace Catalog.Products.Features.GetProductById;

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ProductsRoot}/{{id:guid}}", GetProductById)
            .WithTags(ProductsTag)
            .WithName(nameof(GetProductById));
    }

    private static async Task<EndpointResult<ProductDto>> GetProductById(Guid id, ISender sender)
    {
        GetProductByIdQuery query = new(id);
        return await sender.Send(query);
    }
}
