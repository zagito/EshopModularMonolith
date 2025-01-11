namespace Basket.ShoppingCarts.Features.CreateShoppingCart;

public class CreateShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/shopping-carts", CreateShoppingCart)
            .WithTags("ShoppingCart")
            .WithName("CreateShoppingCart");

        static async Task<EndpointResult<Guid>> CreateShoppingCart(CreateShoppingCartCommand request, ISender sender)
        {
            return await sender.Send(request);
        }
    }
}
