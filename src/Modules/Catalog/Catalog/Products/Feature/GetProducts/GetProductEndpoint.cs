using Shared.Pagination;

namespace Catalog.Products.Feature.GetProducts;

public record GetProductsResponse(PaginatedResult<ProductDto> Products);

public class GetProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var products = await sender.Send(new GetProductsQuery(request));

            if (products is null || products.Products.Items.Count == 0)
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
