using Shared.Exceptions;

namespace Order.Orders.Exceptions;
public class OrderNotFoundException(Guid id)
    : NotFoundException("Order", id)
{
}
