﻿

namespace Basket.ShoppingCarts.Features.CreateShoppingCart;

internal record CreateShoppingCartRequest(List<ShoppingCartItem> Items);

public class CreateShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ShoppingCartsRoot, CreateShoppingCart)
            .WithTags(ShoppingCartsTag)
            .WithName(nameof(CreateShoppingCart))
            .RequireAuthorization();
    }

    private static async Task<EndpointResult<Guid>> CreateShoppingCart(CreateShoppingCartRequest request, ISender sender, ClaimsPrincipal principal)
    {
        var command = new CreateShoppingCartCommand(principal.GetEmail(), request.Items);
        return await sender.Send(command);
    }
}
