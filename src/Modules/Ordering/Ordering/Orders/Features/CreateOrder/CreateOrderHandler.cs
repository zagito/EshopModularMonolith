namespace Ordering.Orders.Features.CreateOrder;

internal record CreateOrderCommand(
    Guid CustomerId,
    string OrderName,
    AddressDto ShippingAddress,
    AddressDto BillingAddress,
    PaymentDto Payment,
    List<CreateOrderItem> Items) : ICommand<Guid>;

internal record CreateOrderItem(Guid ProductId, int Quantity, decimal Price);

internal class CreteOrderItemValidator : AbstractValidator<CreateOrderItem>
{
    public CreteOrderItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity should be greater than 0");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
    }
}

internal class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required");
        RuleFor(x => x.AddressLine).NotEmpty().WithMessage("AddressLine is required");
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
        RuleFor(x => x.State).NotEmpty().WithMessage("State is required");
        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("ZipCode is required");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("EmailAddress is required")
            .EmailAddress().WithMessage("Invalid Email Address");
    }
}

internal class PaymentDtoValidator : AbstractValidator<PaymentDto>
{
    public PaymentDtoValidator()
    {
        RuleFor(x => x.CardName).NotEmpty().WithMessage("CardName is required");
        RuleFor(x => x.CardNumber).NotEmpty().WithMessage("CardNumber is required");
        RuleFor(x => x.Expiration).NotEmpty().WithMessage("Expiration is required");
        RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("PaymentMethod is required");

        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("Cvv is required")
            .Length(3);
    }
}

internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderName).NotEmpty().WithMessage("OrderName is required");
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required");

        RuleFor(x => x.ShippingAddress)
            .NotNull().WithMessage("ShippingAddress is required")
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.BillingAddress)
            .NotNull().WithMessage("BillingAddress is required")
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.Payment)
            .NotNull().WithMessage("Payment is required")
            .SetValidator(new PaymentDtoValidator());

        RuleFor(x => x.Items)
            .NotNull().WithMessage("Items is required")
            .Must(x => x.Count > 0).WithMessage("Items should have at least one item")
            .ForEach(x => x.SetValidator(new CreteOrderItemValidator()));
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
