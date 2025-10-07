using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketResponse(Guid BasketId);

public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/baskets/{userName}/items/{productId}",
            async ([FromRoute] string userName,
            [FromRoute] Guid productId,
            ISender sender) =>
        {
            var command = new RemoveItemFromBasketCommand(userName, productId);
            var response = await sender.Send(command);
            return response.BasketId != Guid.Empty ? Results.Ok(response) : Results.NotFound();
        })
        .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Remove item from basket")
        .WithDescription("Removes an item from the user's shopping basket.");
    }
}
