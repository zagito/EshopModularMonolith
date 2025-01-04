namespace Catalog.Products.Features.CreateProduct;

public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    decimal Price,
    string ImageFile);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", CreateProduct)
            .WithTags("Products")
            .WithName("CreateProduct");

        static async Task<ModelResult<Guid>> CreateProduct(CreateProductRequest request, ISender sender)
        {
            CreateProductCommand command = request.Adapt<CreateProductCommand>();
            return await sender.Send(command);

            //return Results.Created($"/products/{productId.Value}", productId.Value);
        }
    }
}
