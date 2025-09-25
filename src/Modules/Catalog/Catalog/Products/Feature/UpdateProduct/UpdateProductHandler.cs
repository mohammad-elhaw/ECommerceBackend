namespace Catalog.Products.Feature.UpdateProduct;

public record UpdateProductCommand(ProductDto Product)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator
    : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id)
            .NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.Product.Name)
            .NotEmpty().WithMessage("Product name is required.");
        RuleFor(x => x.Product.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}

public class UpdateProductHandler(CatalogDbContext context)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Product? product = await context.Products
            .FindAsync([command.Product.Id] , cancellationToken: cancellationToken) 
            ?? throw new ProductNotFoundException(command.Product.Id);
        
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
