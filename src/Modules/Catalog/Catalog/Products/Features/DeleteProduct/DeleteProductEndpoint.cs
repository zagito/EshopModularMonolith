namespace Catalog.Products.Features.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ProductsRoot}/{{id:guid}}", DeleteProduct)
            .WithTags(ProductsTag)
            .WithName(nameof(DeleteProduct));
    }

    private static async Task<EndpointResult> DeleteProduct(Guid id, ISender sender)
    {
        DeleteProductCommand command = new(id);
        return await sender.Send(command);
    }
}
