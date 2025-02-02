using Shared.Pagination;

namespace Ordering.Orders.Features.GetOrders;

internal class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", GetOrders)
            .WithTags("Orders")
            .WithName(nameof(GetOrders));
    }

    private static async Task<EndpointResult<PaginationResult<OrderDto>>> GetOrders
        ([AsParameters] PaginationRequest request, ISender sender)
    {
        var query = new GetOrdersQuery(request);
        return await sender.Send(query);
    }
}

