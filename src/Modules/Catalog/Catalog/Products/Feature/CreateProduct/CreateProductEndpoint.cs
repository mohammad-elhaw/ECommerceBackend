namespace Catalog.Products.Feature.CreateProduct;

public record CreateProductRequest(ProductDto Product);
public record CreateProductResponse(Guid ProductId);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateProductCommand(request.Product));
            return Results.Created($"/products/{result.ProductId}", 
                new CreateProductResponse(result.ProductId));
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new product")
        .WithDescription("Creates a new product with the provided details.")
        .ProducesValidationProblem();
    }
}
