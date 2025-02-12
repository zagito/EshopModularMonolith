using Basket.ShoppingCarts.Models;
using CatalogContracts.Products.Features.GetProductById;

namespace Basket.ShoppingCarts.Features.CreateShoppingCart;

internal record CreateShoppingCartCommand(
    string UserName,
    List<ShoppingCartItem> Items) : ICommand<Guid>;

internal record ShoppingCartItem(
    Guid ProductId,
    int Quantity,
    string Color);

internal class CreateShoppingCartCommandValidator : AbstractValidator<CreateShoppingCartCommand>
{
    public CreateShoppingCartCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Items).NotNull();
        RuleFor(x => x.Items).Must(x => x.Count > 0);
    }
}

internal class CreateShoppingCartHandler(IShoppingCartRepository repository, ISender sender) : ICommandHandler<CreateShoppingCartCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateShoppingCartCommand command, CancellationToken cancellationToken)
    {
        var shoppingCartResult = await CreateShoppingCart(command);

        if (shoppingCartResult.IsFailure)
            return shoppingCartResult.Error!;

        var userShopingCart = await repository.GetAsync(command.UserName);
        if (userShopingCart != null)
            return Error.ShoppingCartAlreadyCreated;

        var result = await repository.CreateAsync(shoppingCartResult.Value!, cancellationToken);
        return result.Id;
    }

    private async Task<Result<ShoppingCart>> CreateShoppingCart(CreateShoppingCartCommand dto)
    {
        var shoppingCart = ShoppingCart.Create(dto.UserName);

        foreach (var item in dto.Items)
        {
            var productResult = await sender.Send(new GetProductByIdQuery(item.ProductId));

            if (productResult.IsFailure)
                return productResult.Error!;

            shoppingCart.AddItem(item.ProductId, productResult.Value!.Name, productResult.Value!.Price, item.Quantity, item.Color);
        }

        return shoppingCart;
    }
}
