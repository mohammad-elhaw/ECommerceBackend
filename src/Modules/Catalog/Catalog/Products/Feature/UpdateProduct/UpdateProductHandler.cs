namespace Catalog.Products.Feature.UpdateProduct;

public record UpdateProductCommand(ProductDto Product)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductHandler(CatalogDbContext context)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Product? product = await context.Products
            .FindAsync([command.Product.Id] , cancellationToken: cancellationToken);

        if (product is null)
            throw new Exception($"Product not found: {command.Product.Id}");


        UpdateProductWithNewValues(product, command.Product);

        context.Update(product);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private static void UpdateProductWithNewValues(Product product, ProductDto commandProduct) =>
        product.Update(
            commandProduct.Name,
            commandProduct.Categories,
            commandProduct.Description,
            commandProduct.ImageFile,
            commandProduct.Price);
    
}
