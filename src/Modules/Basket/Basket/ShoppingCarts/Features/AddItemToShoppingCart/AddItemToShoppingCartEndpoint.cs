namespace Basket.ShoppingCarts.Features.AddItemToShoppingCart;

public class AddItemToShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/shopping-carts/{userName}/items", AddItemToShoppingCart)
           .WithTags("ShoppingCart")
           .WithName("AddItemToShoppingCart");

        static async Task<EndpointResult<Guid>> AddItemToShoppingCart
            ([FromRoute] string userName, [FromBody] ShoppingCartItem item, ISender sender)
        {
            var command = new AddItemToShoppingCartCommand(userName, item);
            return await sender.Send(command);
        }
    }
}