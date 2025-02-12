namespace Ordering.Orders.Features.CreateOrder;

internal class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(OrdersRoute, CreateOrder)
        .WithTags(OrdersTag)
        .WithName("CreateOrder"); 
    }

    private static async Task<EndpointResult> CreateOrder([FromBody] CreateOrderCommand command, ISender sender) 
        => await sender.Send(command);
}
