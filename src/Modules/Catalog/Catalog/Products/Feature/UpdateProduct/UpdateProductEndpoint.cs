namespace Catalog.Products.Feature.UpdateProduct;

public record UpdateProductRequest(ProductDto Product);
public record UpdateProductResponse(bool IsSuccess);
public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateProductCommand(request.Product));
            return Results.Ok(new UpdateProductResponse(result.IsSuccess));
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Updates an existing product")
        .WithDescription("Updates an existing product with the provided details.")
        .ProducesValidationProblem();
    }
}
