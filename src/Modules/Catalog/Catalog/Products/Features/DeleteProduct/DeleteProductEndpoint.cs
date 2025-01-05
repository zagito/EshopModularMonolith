namespace Catalog.Products.Features.DeleteProduct;

//public record DeleteProductRequest(Guid Id);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", DeleteProduct)
            .WithTags("Products")
            .WithName("DeleteProduct");

        static async Task<EndpointResult> DeleteProduct(Guid id, ISender sender)
        {
            DeleteProductCommand command = new(id);
            return await sender.Send(command);
        }
    }
}
