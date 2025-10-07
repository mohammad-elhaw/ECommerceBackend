using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(ShoppingCartItemDto ShoppingCartItem);
public record AddItemIntoBasketResponse(Guid Id);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/baskets/{userName}/items",
            async ([FromRoute] string userName,
            [FromBody] AddItemIntoBasketRequest request,
            ISender sender) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);
            var response = await sender.Send(command);
            return Results.Created($"/baskets/{response.Id}", response);
        })
        .Produces<AddItemIntoBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add item into basket")
        .WithDescription("Adds an item into the user's shopping basket.");
    }
}
