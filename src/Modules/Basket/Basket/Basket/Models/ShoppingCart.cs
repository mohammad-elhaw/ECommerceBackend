namespace Basket.Basket.Models;
public class ShoppingCart : Aggregate<Guid>
{
    public string UserName { get; private set; } = default!;

    private readonly List<ShoppingCartItem> items = [];
    public IReadOnlyList<ShoppingCartItem> Items => items.AsReadOnly();

    public decimal TotalPrice => items.Sum(item => item.Price * item.Quantity);

    private ShoppingCart() { }

    public static ShoppingCart Create(Guid id, string userName)
    {
        ArgumentException.ThrowIfNullOrEmpty(userName);
        return new ShoppingCart
        {
            Id = id,
            UserName = userName
        };
    }

    public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = items.FirstOrDefault(i => i.ProductId == productId);

        if(existingItem is not null)
            existingItem.Quantity += quantity;

        else
        {
            var newItem = new ShoppingCartItem(Id, productId, quantity, color, price, productName);
            items.Add(newItem);
        }
    }

    public void RemoveItem(Guid productId)
    {
        var item = items.FirstOrDefault(i => i.ProductId == productId);
        if(item is not null) items.Remove(item);
    }
}
