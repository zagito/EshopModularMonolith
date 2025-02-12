namespace Basket.ShoppingCarts.Features.RemoveItemFromShoppingCart;

internal class RemoveItemFromShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ShoppingCartsRoot}/items/{{productId}}", RemoveItemFromShoppingCart)
            .WithTags(ShoppingCartsTag)
            .WithName(nameof(RemoveItemFromShoppingCart));
    }

    private static async Task<EndpointResult<Guid>> RemoveItemFromShoppingCart
            ([FromRoute] Guid productId, ISender sender, ClaimsPrincipal principal)
    {
        var command = new RemoveItemFromShoppingCartCommand(principal.GetEmail(), productId);
        return await sender.Send(command);
    }
}
