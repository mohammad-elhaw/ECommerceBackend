namespace Catalog.Products.Feature.GetProductById;

internal class GetProductByIdHandler(CatalogDbContext context)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        return product is null
            ? throw new ProductNotFoundException(query.Id)
            : new GetProductByIdResult(new ProductDto(
            product.Id,
            product.Name,
            product.Categories,
            product.Description,
            product.ImageFile,
            product.Price));
    }
}
