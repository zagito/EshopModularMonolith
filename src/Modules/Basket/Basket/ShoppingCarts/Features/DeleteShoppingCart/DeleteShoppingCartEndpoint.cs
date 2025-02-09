namespace Basket.ShoppingCarts.Features.DeleteShoppingCart;

public class DeleteShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ShoppingCartsRoot}/{{userName}}", DeleteShoppingCart)
            .WithTags(ShoppingCartsTag)
            .WithName(nameof(DeleteShoppingCart));
    }

    private static async Task<EndpointResult> DeleteShoppingCart(string userName, ISender sender)
    {
        var command = new DeleteShoppingCartCommand(userName);
        return await sender.Send(command);
    }
}
