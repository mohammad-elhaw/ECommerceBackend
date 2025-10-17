using System.Text.Json.Serialization;

namespace Basket.Basket.Models;
public class ShoppingCartItem : Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; internal set; }
    public string Color { get; private set; }

    // this will be get from Catalog module
    public decimal Price { get; private set; }
    public string ProductName { get; private set; }


    internal ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, string color,
        decimal price, string productName)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductName = productName;
    }

    [JsonConstructor]
    internal ShoppingCartItem(Guid id, Guid shoppingCartId, Guid productId, int quantity, string color,
        decimal price, string productName) : this(shoppingCartId, productId, quantity, color, price, productName)
    {
        Id = id;
    }

    public void UpdatePrice(decimal newPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);
        Price = newPrice;
    }
}
