namespace Catalog.Products.Feature.GetProducts;

public record GetProductsResponse(List<ProductDto> Products);

public class GetProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var products = await sender.Send(new GetProductsQuery());

            if (products is null || products.Products.Count == 0)
                return Results.NoContent();

            return Results.Ok(new GetProductsResponse(products.Products));
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Gets all products")
        .WithDescription("Gets all products from the catalog.");
    }
}
