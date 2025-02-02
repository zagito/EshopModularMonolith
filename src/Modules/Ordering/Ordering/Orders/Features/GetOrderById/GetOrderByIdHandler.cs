namespace Ordering.Orders.Features.GetOrderById;

internal record GetOrderByIdQuery(Guid Id) : IQuery<OrderDto>;

internal class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

internal class GetOrderByIdHandler(OrderingDbContext dbContext) : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        Order? order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (order is null)
            return Error.OrderNotFound;

        return order.Adapt<OrderDto>();
    }
}
