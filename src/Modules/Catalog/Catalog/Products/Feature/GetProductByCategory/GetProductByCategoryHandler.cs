namespace Catalog.Products.Feature.GetProductByCategory;

public record GetProductByCategoryQeury(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(List<ProductDto> Products);

internal class GetProductByCategoryHandler(CatalogDbContext context)
    : IQueryHandler<GetProductByCategoryQeury, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQeury query, CancellationToken cancellationToken)
    {
        var products = await context.Products.AsNoTracking()
            .Where(p => p.Categories.Contains(query.Category))
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Categories,
                p.Description,
                p.ImageFile,
                p.Price)).ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}
