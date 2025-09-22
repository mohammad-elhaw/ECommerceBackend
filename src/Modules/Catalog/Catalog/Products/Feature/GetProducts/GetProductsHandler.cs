namespace Catalog.Products.Feature.GetProducts;

public record GetProductsQuery : IQuery<GetProductsResult>;
public record GetProductsResult(List<ProductDto> Products);

internal class GetProductsHandler (CatalogDbContext context)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await context.Products.AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Categories,
                p.Description,
                p.ImageFile,
                p.Price))
            .ToListAsync(cancellationToken);

        return new GetProductsResult(products);
    }
}
