using Basket.ShoppingCarts.Models;
using Microsoft.Extensions.Logging;
using SharedMessaging.IntegrationEvents;
using System.Text.Json;

namespace Basket.ShoppingCarts.Features.CheckoutShoppingCart;

internal record CheckoutShoppingCartCommand(
    string UserName,
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
    int PaymentMethod) : ICommand;

internal class CheckoutShoppingCartCommandValidator : AbstractValidator<CheckoutShoppingCartCommand>
{
    public CheckoutShoppingCartCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
        RuleFor(x => x.AddressLine).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
        RuleFor(x => x.CardName).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty();
        RuleFor(x => x.Expiration).NotEmpty();
        RuleFor(x => x.PaymentMethod).NotEmpty();
    }
}

internal class CheckoutShoppingCartHandler(BasketDbContext dbContext, ILogger<CheckoutShoppingCartHandler> logger) : ICommandHandler<CheckoutShoppingCartCommand>
{
    public async Task<Result> Handle(CheckoutShoppingCartCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var shoppingCart = await dbContext.ShoppingCarts
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

            if (shoppingCart is null)
                return Error.ShoppingCartNotFound;

            ShoppingCartCheckoutIntegrationEvent eventMessage = command.Adapt<ShoppingCartCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = shoppingCart.TotalPrice;
            eventMessage.Items = shoppingCart.Items.Select(x => new ShoppingCartItemDto
            (
                x.ProductId,
                x.ProductName,
                x.Price,
                x.Quantity
            )).ToList();

            OutboxMessage outboxMessage = OutboxMessage.Create(
                typeof(ShoppingCartCheckoutIntegrationEvent).AssemblyQualifiedName!,
                JsonSerializer.Serialize(eventMessage));

            dbContext.OutboxMessages.Add(outboxMessage);
            dbContext.ShoppingCarts.Remove(shoppingCart);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CheckoutShoppingCartCommandHandler - error");
            await transaction.RollbackAsync(cancellationToken);
            return Error.ShoppingCartCheckoutFailed;
        }

        

        //var shoppingCart = await repository.GetAsync(command.UserName, true, cancellationToken);

        //if (shoppingCart is null)
        //    return Error.ShoppingCartNotFound;

        //ShoppingCartCheckoutIntegrationEvent shoppingCartCheckoutIntegrationEvent = command.Adapt<ShoppingCartCheckoutIntegrationEvent>();
        //await publishEndpoint.Publish(shoppingCartCheckoutIntegrationEvent, cancellationToken); 

        //await repository.DeleteAsync(command.UserName, cancellationToken);

        //return Result.Success();
    }
}
