namespace Catalog.Products.Feature.GetProductByCategory;

public record GetProductByCategoryRequest(string Category);
public record GetProductByCategoryResponse(List<ProductDto> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("products/category/{category}", async (string category, ISender sender) =>
        {
           var products = await sender
            .Send(new GetProductByCategoryQeury(category));

            return Results.Ok(new GetProductByCategoryResponse(products.Products));
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products by Category")
        .WithDescription("Get Products by Category");
    }
}
