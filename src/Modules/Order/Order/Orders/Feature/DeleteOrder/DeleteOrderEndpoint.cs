namespace Order.Orders.Feature.DeleteOrder;

public record DeleteOrderResponse(bool Success);

public class DeleteOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{orderId:guid}", async (Guid orderId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteOrderCommand(orderId));
            return Results.Ok(new DeleteOrderResponse(result.IsDeleted));
        }).WithName("DeleteOrder")
          .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Deletes an order by its unique identifier.")
          .WithDescription("Deletes an order by its unique identifier.");
    }
}
