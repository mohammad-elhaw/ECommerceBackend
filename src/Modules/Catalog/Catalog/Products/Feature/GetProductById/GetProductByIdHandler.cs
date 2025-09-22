namespace Catalog.Products.Feature.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(ProductDto Product);

internal class GetProductByIdHandler(CatalogDbContext context)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        return product is null
            ? throw new KeyNotFoundException($"Product with Id {query.Id} not found.")
            : new GetProductByIdResult(new ProductDto(
            product.Id,
            product.Name,
            product.Categories,
            product.Description,
            product.ImageFile,
            product.Price));
    }
}
