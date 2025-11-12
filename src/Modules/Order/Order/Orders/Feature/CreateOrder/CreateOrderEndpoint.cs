namespace Order.Orders.Feature.CreateOrder;

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid OrderId);

public class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateOrderCommand(request.Order));
            return Results.Created($"/orders/{result.OrderId}",
                new CreateOrderResponse(result.OrderId));
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create a new order")
        .WithDescription("Creates a new order with the provided details.");
    }
}
