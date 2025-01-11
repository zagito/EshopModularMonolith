namespace Basket.ShoppingCarts.Features.RemoveItemFromShoppingCart;

internal record RemoveItemFromShoppingCartCommand(string UserName, Guid ProductId) : ICommand<Guid>;

internal class RemoveItemFromShoppingCartCommandValidator : AbstractValidator<RemoveItemFromShoppingCartCommand>
{
    public RemoveItemFromShoppingCartCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}

internal class RemoveItemFromShoppingCartHandler(IShoppingCartRepository repository) 
    : ICommandHandler<RemoveItemFromShoppingCartCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RemoveItemFromShoppingCartCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetAsync(command.UserName, false, cancellationToken);

        if (shoppingCart is null)
            return Error.ShoppingCartNotFound;

        shoppingCart.RemoveItem(command.ProductId);

        await repository.SaveChangesAsync(command.UserName, cancellationToken);

        return shoppingCart.Id;
    }
}
