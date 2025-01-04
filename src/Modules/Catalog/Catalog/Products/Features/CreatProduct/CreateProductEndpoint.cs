namespace Catalog.Products.Features.CreatProduct;

public record CreatProductRequest(
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

        static async Task<ModelResult<Guid>> CreateProduct(CreatProductRequest request, ISender sender)
        {
            CreatProductComand comand = request.Adapt<CreatProductComand>();
            return await sender.Send(comand);

            //return Results.Created($"/products/{productId.Value}", productId.Value);
        }
    }
}
