namespace Basket.ShoppingCarts.Features.UpdateItemPriceInShoppingCart;

internal record UpdateItemPriceInShoppingCartCommand(Guid ProductId, decimal Price) 
    : ICommand;

internal class UpdateItemPriceInShoppingCartValidator : AbstractValidator<UpdateItemPriceInShoppingCartCommand>
{
    public UpdateItemPriceInShoppingCartValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateItemPriceInShoppingCartHandler(BasketDbContext dbContext) : ICommandHandler<UpdateItemPriceInShoppingCartCommand>
{
    public async Task<Result> Handle(UpdateItemPriceInShoppingCartCommand command, CancellationToken cancellationToken)
    {
        var shoppingCartItems = await dbContext.ShoppingCartItems
            .Where(x => x.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);

        foreach (var shoppingCartItem in shoppingCartItems)
        {
            shoppingCartItem.UpdatePrice(command.Price);
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}