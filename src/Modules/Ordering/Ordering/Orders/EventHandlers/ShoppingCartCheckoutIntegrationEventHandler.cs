using MassTransit;
using Ordering.Orders.Features.CreateOrder;
using SharedMessaging.IntegrationEvents;

namespace Ordering.Orders.EventHandlers;

public class ShoppingCartCheckoutIntegrationEventHandler(ISender sender,
    ILogger<ShoppingCartCheckoutIntegrationEventHandler> logger) : IConsumer<ShoppingCartCheckoutIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ShoppingCartCheckoutIntegrationEvent> context)
    {
        logger.LogInformation("Integration event handled : {IntegrationEvent}", context.Message);
        CreateOrderCommand command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
    }

    private static CreateOrderCommand MapToCreateOrderCommand(ShoppingCartCheckoutIntegrationEvent message)
    {
        AddressDto shippingAddress = CreateAddress(message);
        AddressDto billingAddress = CreateAddress(message);
        PaymentDto payment = CreatePayment(message);

        return new CreateOrderCommand(
            CustomerId: message.UserId,
            OrderName: message.UserName,
            ShippingAddress: shippingAddress,
            BillingAddress: billingAddress,
            Payment: payment,
            Items: message.Items.Select(x => new CreateOrderItem(x.ProductId, x.Quantity, x.UnitPrice)).ToList()
        );
    }

    private static PaymentDto CreatePayment(ShoppingCartCheckoutIntegrationEvent eventMessage)
    {
        return new(
            eventMessage.CardName,
            eventMessage.CardNumber,
            eventMessage.Expiration,
            eventMessage.Cvv,
            eventMessage.PaymentMethod);
    }

    private static AddressDto CreateAddress(ShoppingCartCheckoutIntegrationEvent eventMessage)
    {
        return new(
            eventMessage.FirstName,
            eventMessage.LastName,
            eventMessage.EmailAddress,
            eventMessage.AddressLine,
            eventMessage.Country,
            eventMessage.State,
            eventMessage.ZipCode);
    }
}
