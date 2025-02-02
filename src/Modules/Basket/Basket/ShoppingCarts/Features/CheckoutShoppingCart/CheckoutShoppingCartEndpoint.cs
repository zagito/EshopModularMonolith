
namespace Basket.ShoppingCarts.Features.CheckoutShoppingCart;

public class CheckoutShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/shopping-carts/checkout", CheckoutShoppingCart)
           .WithTags("ShoppingCart")
           .WithName("CheckoutShoppingCart");
    }

    private static async Task<EndpointResult> CheckoutShoppingCart(
        [FromBody] CheckoutShoppingCartCommand request, ISender sender)
    {
        return await sender.Send(request);
    }
}
