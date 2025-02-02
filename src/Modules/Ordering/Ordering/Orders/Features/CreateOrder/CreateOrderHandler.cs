namespace Ordering.Orders.Features.CreateOrder;

internal record CreateOrderCommand(
    Guid CustomerId,
    string OrderName,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    PaymentDto Payment,
    List<CreateOrderItem> Items) : ICommand<Guid>;

internal record CreateOrderItem(Guid ProductId, int Quantity, decimal Price);

internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderName).NotEmpty().WithMessage("OrderName is required");
    }
}

internal class CreateOrderHandler(OrderingDbContext dbContext) : ICommandHandler<CreateOrderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        Order order = CreateOrder(command);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return order.Id;
    }

    private static Order CreateOrder(CreateOrderCommand command)
    {
        Address shippingAddress = CreateAddress(command.ShippingAddress);
        Address billingAddress = CreateAddress(command.BillingAddress);
        Payment payment = CreatePayment(command.Payment);

        var order = Order.Create(
            customerId: command.CustomerId,
            orderName: $"{command.OrderName}_{new Random().Next()}",
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: payment
        );

        foreach (var item in command.Items)
        {
            order.Add(item.ProductId, item.Quantity, item.Price);
        }

        return order;
    }

    private static Payment CreatePayment(PaymentDto payment)
    {
        return Payment.Create(
            payment.CardName,
            payment.CardNumber,
            payment.Expiration,
            payment.Cvv,
            payment.PaymentMethod);
    }

    private static Address CreateAddress(AddressDto addressDto)
    {
        return Address.Create(
            addressDto.FirstName,
            addressDto.LastName,
            addressDto.EmailAddress,
            addressDto.AddressLine,
            addressDto.Country,
            addressDto.State,
            addressDto.ZipCode);
    }
}
