using Shared.Pagination;

namespace Ordering.Orders.Features.GetOrders;

internal record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<PaginationResult<OrderDto>>;

internal class GetOrdersHandler(OrderingDbContext dbContext) : IQueryHandler<GetOrdersQuery, PaginationResult<OrderDto>>
{
    public async Task<Result<PaginationResult<OrderDto>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        List<Order> orders = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .OrderBy(p => p.OrderName)
            .Skip(query.PaginationRequest.PageIndex * query.PaginationRequest.PageSize)
            .Take(query.PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        List<OrderDto> orderDtos = orders.Adapt<List<OrderDto>>();

        return new PaginationResult<OrderDto>(
            query.PaginationRequest.PageIndex,
            query.PaginationRequest.PageSize,
            totalCount,
            orderDtos);
    }
}
