using System.Security.Claims;

namespace Basket.ShoppingCarts.Features.CreateShoppingCart;

internal record CreateShoppingCartRequest(List<ShoppingCartItem> Items);

public class CreateShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/shopping-carts", CreateShoppingCart)
            .WithTags("ShoppingCart")
            .WithName("CreateShoppingCart")
            .RequireAuthorization();

        static async Task<EndpointResult<Guid>> CreateShoppingCart(CreateShoppingCartRequest request, ISender sender, ClaimsPrincipal principal)
        {
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var command = new CreateShoppingCartCommand(email!, request.Items);
            return await sender.Send(command);
        }
    }
}
