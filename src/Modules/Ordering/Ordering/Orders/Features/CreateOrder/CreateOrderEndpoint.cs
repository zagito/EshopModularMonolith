namespace Ordering.Orders.Features.CreateOrder;

internal class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(OrdersRoute, async ([FromBody] CreateOrderCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsSuccess
                ? Results.Created($"/orders/{result.Value}", result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithTags(OrdersTag)
        .WithName("CreateOrder"); 
    }
}
