namespace Basket.ShoppingCarts.Features.DeleteShoppingCart;

internal record DeleteShoppingCartCommand(string UserName) : ICommand;

internal class DeleteShoppingCartCommandValidator : AbstractValidator<DeleteShoppingCartCommand>
{
    public DeleteShoppingCartCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
    }
}

internal class DeleteShoppingCartHandler(IShoppingCartRepository repository) : ICommandHandler<DeleteShoppingCartCommand>
{
    public async Task<Result> Handle(DeleteShoppingCartCommand command, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteAsync(command.UserName, cancellationToken);
        return result ? Result.Success() : Error.ShoppingCartNotFound;
    }
}
