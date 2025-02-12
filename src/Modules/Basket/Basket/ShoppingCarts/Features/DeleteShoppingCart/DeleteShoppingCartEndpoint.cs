namespace Basket.ShoppingCarts.Features.DeleteShoppingCart;

public class DeleteShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(ShoppingCartsRoot, DeleteShoppingCart)
            .WithTags(ShoppingCartsTag)
            .WithName(nameof(DeleteShoppingCart))
            .RequireAuthorization();
    }

    private static async Task<EndpointResult> DeleteShoppingCart(ISender sender, ClaimsPrincipal principal)
    {
        var command = new DeleteShoppingCartCommand(principal.GetEmail());
        return await sender.Send(command);
    }
}
