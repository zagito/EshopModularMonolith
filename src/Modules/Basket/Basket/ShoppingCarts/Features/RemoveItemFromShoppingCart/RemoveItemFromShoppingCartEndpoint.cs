namespace Basket.ShoppingCarts.Features.RemoveItemFromShoppingCart;

internal class RemoveItemFromShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ShoppingCartsRoot}/{{userName}}/items/{{productId}}", RemoveItemFromShoppingCart)
            .WithTags(ShoppingCartsTag)
            .WithName(nameof(RemoveItemFromShoppingCart));
    }

    private static async Task<EndpointResult<Guid>> RemoveItemFromShoppingCart
            ([FromRoute] string userName, [FromRoute] Guid productId, ISender sender)
    {
        var command = new RemoveItemFromShoppingCartCommand(userName, productId);
        return await sender.Send(command);
    }
}
