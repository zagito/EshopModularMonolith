namespace Basket.ShoppingCarts.Features.AddItemToShoppingCart;

public class AddItemToShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{ShoppingCartsRoot}/items", AddItemToShoppingCart)
           .WithTags(ShoppingCartsTag)
           .WithName(nameof(AddItemToShoppingCart))
           .RequireAuthorization();
    }

    private static async Task<EndpointResult<Guid>> AddItemToShoppingCart
            ([FromBody] ShoppingCartItem item, ISender sender, ClaimsPrincipal principal)
    {
        var command = new AddItemToShoppingCartCommand(principal.GetEmail(), item);
        return await sender.Send(command);
    }
}