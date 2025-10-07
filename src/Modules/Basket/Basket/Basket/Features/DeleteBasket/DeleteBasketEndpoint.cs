namespace Basket.Basket.Features.DeleteBasket;

public record DeleteBasketResponse(bool IsSuccess);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/baskets/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(userName));
            return result.IsSuccess 
                ? Results.Ok(new DeleteBasketResponse(true)) 
                : Results.NotFound(new DeleteBasketResponse(false));
        })
        .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
        .Produces<DeleteBasketResponse>(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Deletes a shopping basket.")
        .WithDescription("Deletes the shopping basket for the specified user.");
    }
}
