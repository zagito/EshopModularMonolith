namespace Ordering.Orders.Features.DeleteOrder;

public class DeleteOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{orderId:guid}", DeleteOrder)
            .WithTags("Orders")
            .WithName(nameof(DeleteOrder));
    }

    private static async Task<EndpointResult> DeleteOrder([FromRoute] Guid orderId, ISender sender)
    {
        DeleteOrderCommand command = new(orderId);
        return await sender.Send(command);
    }
}
