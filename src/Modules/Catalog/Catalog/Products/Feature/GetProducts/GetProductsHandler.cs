using Shared.Pagination;

namespace Catalog.Products.Feature.GetProducts;

public record GetProductsQuery(PaginationRequest paginationRequest) 
    : IQuery<GetProductsResult>;
public record GetProductsResult(PaginatedResult<ProductDto> Products);

internal class GetProductsHandler (CatalogDbContext context)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        int pageNumber = query.paginationRequest.PageNumber < 1 ? 1 : query.paginationRequest.PageNumber;
        int pageSize = query.paginationRequest.PageSize < 1 ? 10 : query.paginationRequest.PageSize;
        long totalRecords = await context.Products.LongCountAsync(cancellationToken);
        long totalPages = (long)Math.Ceiling(totalRecords / (double)pageSize);

        var products = await context.Products.AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Categories,
                p.Description,
                p.ImageFile,
                p.Price))
            .ToListAsync(cancellationToken);

        return new GetProductsResult(
            new PaginatedResult<ProductDto>(
                pageNumber, 
                pageSize, 
                totalRecords, 
                totalPages, 
                products)
            );
    }
}
