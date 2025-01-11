namespace Basket.ShoppingCarts.Features.GetShoppingCart;

public class GetShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/shopping-carts/{userName}", GetShoppingCart)
            .WithTags("ShoppingCart")
            .WithName("GetShoppingCart");

        static async Task<EndpointResult<ShoppingCartResponse>> GetShoppingCart(string userName, ISender sender)
        {
            var query = new GetShoppingCartQuery(userName);
            return await sender.Send(query);
        }
    }
}
