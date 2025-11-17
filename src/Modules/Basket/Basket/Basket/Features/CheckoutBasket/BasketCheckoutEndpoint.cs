namespace Basket.Basket.Features.CheckoutBasket;

public record BasketCheckoutRequest(BasketCheckoutDto BasketCheckout);
public record BasketCheckoutResponse(bool IsSuccess);

public class BasketCheckoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (BasketCheckoutRequest request, ISender sender) =>
        {
            var result = await sender.Send(new BasketCheckoutCommand(request.BasketCheckout));
            return Results.Ok(new BasketCheckoutResponse(result.IsSuccess));
        }).WithName("CheckoutBasket")
        .Produces<BasketCheckoutResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Basket")
        .WithSummary("Checkout a basket")
        .WithDescription("This endpoint allows you to checkout a basket.")
        .RequireAuthorization();
    }
}
