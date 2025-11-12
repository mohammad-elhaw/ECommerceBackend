namespace Order.Orders.Feature.GetOrderById;

public record GetOrderByIdResponse(OrderDto Order);

public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderId:guid}", async (Guid orderId, ISender sender) =>
        {
            var order = await sender.Send(new GetOrderByIdQuery(orderId));
            return Results.Ok(new GetOrderByIdResponse(order.Order));
        }).WithName("Get Order By Id")
          .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithTags("Orders")
          .WithSummary("Get an order by its unique identifier.")
          .WithDescription("Retrieves the details of an order using its unique identifier (GUID).");
    }
}
