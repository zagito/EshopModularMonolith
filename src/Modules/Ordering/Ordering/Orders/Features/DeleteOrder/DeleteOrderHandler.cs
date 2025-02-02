namespace Ordering.Orders.Features.DeleteOrder;

internal record DeleteOrderCommand(Guid OrderId) : ICommand;

internal class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}

internal class DeleteOrderHandler(OrderingDbContext dbContext) : ICommandHandler<DeleteOrderCommand>
{
    public async Task<Result> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        Order? order = await dbContext.Orders
          .FindAsync([command.OrderId], cancellationToken: cancellationToken);

        if (order is null)
        {
            return Error.OrderNotFound;
        }

        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
