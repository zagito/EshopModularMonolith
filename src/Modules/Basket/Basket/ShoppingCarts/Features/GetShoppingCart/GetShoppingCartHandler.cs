namespace Basket.ShoppingCarts.Features.GetShoppingCart;

internal record GetShoppingCartQuery(string UserName) : IQuery<ShoppingCartResponse>;

internal record ShoppingCartResponse(
    Guid Id,
    string UserName,
    List<ShoppingCartItemResponse> Items);

internal record ShoppingCartItemResponse (
    Guid Id,
    int Quantity,
    decimal Price,
    string ProductName,
    Guid ProductId);

internal class GetShoppingCartHandler(IShoppingCartRepository repository) : IQueryHandler<GetShoppingCartQuery, ShoppingCartResponse>
{
    public async Task<Result<ShoppingCartResponse>> Handle(GetShoppingCartQuery query, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetAsync(query.UserName, true, cancellationToken);

        if (shoppingCart is null)
            return Error.ShoppingCartNotFound;

        return shoppingCart.Adapt<ShoppingCartResponse>();
    }
}
