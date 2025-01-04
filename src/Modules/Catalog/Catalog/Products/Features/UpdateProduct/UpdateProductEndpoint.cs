namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products/{id:guid}", UpdateProduct)
            .WithTags("Products")
            .WithName("UpdateProduct");

        static async Task<ModelResult> UpdateProduct(Guid id, UpdateProductRequest request, ISender sender)
        {
            UpdateProductComand comand = request.Adapt<UpdateProductComand>() with { Id = id };
            return await sender.Send(comand);
        }
    }
}