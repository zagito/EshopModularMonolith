namespace Basket.ShoppingCarts.Features.AddItemToShoppingCart;

public class AddItemToShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{ShoppingCartsRoot}/{{userName}}/items", AddItemToShoppingCart)
           .WithTags(ShoppingCartsTag)
           .WithName(nameof(AddItemToShoppingCart));
    }

    private static async Task<EndpointResult<Guid>> AddItemToShoppingCart
            ([FromRoute] string userName, [FromBody] ShoppingCartItem item, ISender sender)
    {
        var command = new AddItemToShoppingCartCommand(userName, item);
        return await sender.Send(command);
    }
}