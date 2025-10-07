using Shared.Exceptions;

namespace Basket.Basket.Exceptions;
public class BasketItemNotFoundException(Guid itemId)
    : NotFoundException("BasketItem", itemId)
{
}
