namespace Ordering.Orders.Features.GetOrderById;

public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{OrdersRoute}/{{orderId:guid}}", GetOrderById)
            .WithTags(OrdersTag)
            .WithName(nameof(GetOrderById));
    }

    private static async Task<EndpointResult<OrderDto>> GetOrderById([FromRoute] Guid orderId, ISender sender)
    {
        var query = new GetOrderByIdQuery(orderId);
        return await sender.Send(query);
    }
}
