using System.Security.Claims;

namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCart);
public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/baskets", async (CreateBasketRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            string userName = user.Identity?.Name ?? throw new InvalidOperationException("User is not authenticated.");
            var updatedBasketCommand = request.ShoppingCart with { UserName = userName };
            var result = await sender.Send(new CreateBasketCommand(updatedBasketCommand));
            return Results.Created($"/baskets/{result.Id}", new CreateBasketResponse(result.Id));
        })
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new shopping basket.")
        .WithDescription("Creates a new shopping basket for a user with the provided items.")
        .RequireAuthorization();
    }
}
