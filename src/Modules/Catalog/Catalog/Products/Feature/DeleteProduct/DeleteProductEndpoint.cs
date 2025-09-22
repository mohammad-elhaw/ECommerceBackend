namespace Catalog.Products.Feature.DeleteProduct;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{productId:guid}", async (Guid productId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(productId));
            return Results.Ok(new DeleteProductResponse(result.IsSuccess));
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Deletes an existing product")
        .WithDescription("Deletes an existing product by its ID.")
        .ProducesValidationProblem();
    }
}
