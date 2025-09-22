namespace Catalog.Products.Feature.CreateProduct;

public record CreateProductCommand(ProductDto Product)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid ProductId);

public class CreateProductHandler(CatalogDbContext context)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = CreateProduct(command.Product);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return new CreateProductResult(product.Id);
    }

    private static Product CreateProduct(ProductDto dto) =>
        Product.Create(
            dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
            dto.Name,
            dto.Categories,
            dto.Description,
            dto.ImageFile,
            dto.Price);
    
}
