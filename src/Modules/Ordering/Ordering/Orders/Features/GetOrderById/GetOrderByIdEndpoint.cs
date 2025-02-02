namespace Ordering.Orders.Features.GetOrderById;

public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderId:guid}", GetOrderById)
            .WithTags("Orders")
            .WithName(nameof(GetOrderById));
    }

    private static async Task<EndpointResult<OrderDto>> GetOrderById([FromRoute] Guid orderId, ISender sender)
    {
        var query = new GetOrderByIdQuery(orderId);
        return await sender.Send(query);
    }
}
