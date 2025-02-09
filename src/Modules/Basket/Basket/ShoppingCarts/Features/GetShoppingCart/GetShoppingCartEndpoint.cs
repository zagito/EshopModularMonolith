namespace Basket.ShoppingCarts.Features.GetShoppingCart;

public class GetShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ShoppingCartsRoot}/{{userName}}", GetShoppingCart)
            .WithTags(ShoppingCartsTag)
            .WithName(nameof(GetShoppingCart));
    }

    private static async Task<EndpointResult<ShoppingCartResponse>> GetShoppingCart(string userName, ISender sender)
    {
        var query = new GetShoppingCartQuery(userName);
        return await sender.Send(query);
    }
}
