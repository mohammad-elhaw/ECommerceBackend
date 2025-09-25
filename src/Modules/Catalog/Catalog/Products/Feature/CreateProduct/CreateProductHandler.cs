namespace Catalog.Products.Feature.CreateProduct;

public record CreateProductCommand(ProductDto Product)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid ProductId);

public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Product.Name)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(x => x.Product.Categories)
            .NotEmpty().WithMessage("At least one category is required.");
        RuleFor(x => x.Product.Description)
            .NotEmpty().WithMessage("Product description is required.");

        RuleFor(x => x.Product.ImageFile)
            .NotEmpty().WithMessage("Product image file is required.");

        RuleFor(x => x.Product.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}

public class CreateProductHandler
    (CatalogDbContext context)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = CreateProduct(command.Product);
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
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
