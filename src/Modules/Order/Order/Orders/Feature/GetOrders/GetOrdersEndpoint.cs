using Shared.Pagination;

namespace Order.Orders.Feature.GetOrders;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var orders = await sender.Send(new GetOrdersQuery(request));
            if (orders is null || orders.Orders.Items.Count == 0)
                return Results.NoContent();
            
            return Results.Ok(new GetOrdersResponse(orders.Orders));
        }).WithName("GetOrders")
          .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status204NoContent)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Get a paginated list of orders")
          .WithDescription("Gets a paginated list of orders based on the provided pagination parameters.");
    }
}
