using Shared.Exceptions;

namespace Catalog.Products.Exceptions;
public sealed class ProductNotFoundException(Guid Id) 
    : NotFoundException("Product", Id)
{
}
