using Shared.CQRS;

namespace Catalog.Feature;

public record CreateProductCommand(
    string Name, List<string> Categories, string Description,
    string ImageFile, decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid ProductId);

public class CreateProductHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
