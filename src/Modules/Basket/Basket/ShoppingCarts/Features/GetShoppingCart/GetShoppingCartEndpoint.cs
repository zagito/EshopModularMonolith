namespace Basket.ShoppingCarts.Features.GetShoppingCart;

public class GetShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ShoppingCartsRoot, GetShoppingCart)
            .WithTags(ShoppingCartsTag)
            .WithName(nameof(GetShoppingCart))
            .RequireAuthorization();
    }

    private static async Task<EndpointResult<ShoppingCartResponse>> GetShoppingCart(ISender sender, ClaimsPrincipal principal)
    {
        var query = new GetShoppingCartQuery(principal.GetEmail());
        return await sender.Send(query);
    }
}
