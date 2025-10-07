namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCart);
public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/baskets", async (CreateBasketRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateBasketCommand(request.ShoppingCart));
            return Results.Created($"/baskets/{result.Id}", new CreateBasketResponse(result.Id));
        })
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new shopping basket.")
        .WithDescription("Creates a new shopping basket for a user with the provided items.");
    }
}
