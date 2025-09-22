namespace Catalog.Products.Feature.GetProductById;

public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{productId:guid}", async (Guid productId, ISender sender) =>
        {
            var product = await sender.Send(new GetProductByIdQuery(productId));

            return Results.Ok(new GetProductByIdResponse(product.Product));
        })
        .WithName("GetProduct")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product")
        .WithDescription("Get Product by Id");
    }
}
