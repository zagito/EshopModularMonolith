using CatalogContracts.Products.Features.GetProductById;

namespace Basket.ShoppingCarts.Features.AddItemToShoppingCart;

internal record AddItemToShoppingCartCommand(string UserName, ShoppingCartItem Item) : ICommand<Guid>;

internal record ShoppingCartItem(
    Guid ProductId,
    int Quantity,
    string Color);

internal class AddItemToShoppingCartHandler(IShoppingCartRepository repository, ISender sender)
    : ICommandHandler<AddItemToShoppingCartCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddItemToShoppingCartCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetAsync(command.UserName, false, cancellationToken);

        if (shoppingCart is null)
            return Error.ShoppingCartNotFound;

        var productResult = await sender.Send(new GetProductByIdQuery(command.Item.ProductId), cancellationToken);

        if (productResult.IsFailure)
            return productResult.Error!;

        shoppingCart.AddItem(
            command.Item.ProductId,
            productResult.Value!.Name,
            productResult.Value!.Price,
            command.Item.Quantity,
            command.Item.Color);

        await repository.SaveChangesAsync(command.UserName, cancellationToken);

        return shoppingCart.Id;
    }
}
