namespace Basket.ShoppingCarts.Features.RemoveItemFromShoppingCart;

internal class RemoveItemFromShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/shopping-carts/{userName}/items/{productId}", RemoveItemFromShoppingCart)
            .WithTags("ShoppingCart")
            .WithName("RemoveItemFromShoppingCart");

        static async Task<EndpointResult<Guid>> RemoveItemFromShoppingCart
            ([FromRoute] string userName, [FromRoute] Guid productId, ISender sender)
        {
            var command = new RemoveItemFromShoppingCartCommand(userName, productId);
            return await sender.Send(command);
        }
    }
}
