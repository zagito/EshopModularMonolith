
namespace Basket.ShoppingCarts.Features.CheckoutShoppingCart;

internal record CheckoutShoppingCartRequest(
    Guid UserId,
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode,
    string CardName,
    string CardNumber,
    string Expiration,
    int PaymentMethod,
    string Cvv
);

public class CheckoutShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost($"{ShoppingCartsRoot}/checkout", CheckoutShoppingCart)
           .WithTags(ShoppingCartsTag)
           .WithName(nameof(CheckoutShoppingCart))
           .RequireAuthorization();
    }

    private static async Task<EndpointResult> CheckoutShoppingCart(
        [FromBody] CheckoutShoppingCartRequest request, ISender sender, ClaimsPrincipal principal)
    {
        CheckoutShoppingCartCommand command = request.Adapt<CheckoutShoppingCartCommand>() with { UserName = principal.GetEmail() };
        return await sender.Send(command);
    }
}
