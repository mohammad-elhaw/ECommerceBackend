using Order.Orders.Events;
using Order.Orders.ValueObjects;
using Shared.DDD;

namespace Order.Orders.Models;
public class Order : Aggregate<Guid>
{
    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Guid CustomerId { get; private set; }
    public string? OrderName { get; private set; }
    public Address? ShippingAddress { get; private set; }
    public Address? BillingAddress { get; private set; }
    public Payment? Payment { get; private set; }
    public decimal TotalAmount => OrderItems.Sum(oi => oi.Price * oi.Quantity);

    private Order() { }

    public static Order Create(Guid id, Guid customerId, string orderName, 
        Address shippingAddress, Address billingAddress, Payment payment)
    {
        Order order = new()
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void Add(Guid productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = _orderItems.FirstOrDefault(oi => oi.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var orderItem = new OrderItem(Id, productId, quantity, price);
            _orderItems.Add(orderItem);
        }
    }

    public void Remove(Guid productId)
    {
        var item = _orderItems.FirstOrDefault(oi => oi.ProductId == productId);
        if(item != null)
        {
            _orderItems.Remove(item);
        }
    }
}
