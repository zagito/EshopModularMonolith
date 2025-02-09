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
        app.MapPut($"{ProductsRoot}/{{id:guid}}", UpdateProduct)
            .WithTags(ProductsTag)
            .WithName(nameof(UpdateProduct));
    }

    private static async Task<EndpointResult> UpdateProduct(Guid id, UpdateProductRequest request, ISender sender)
    {
        UpdateProductCommand command = request.Adapt<UpdateProductCommand>() with { Id = id };
        return await sender.Send(command);
    }
}