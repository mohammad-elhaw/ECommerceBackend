namespace Catalog.Products.Models;
public class Product : Aggregate<Guid>
{
    public string Name { get; private set; } = null!;
    public List<string> Categories { get; private set; } = [];
    public string Description { get; private set; } = null!;
    public string ImageFile { get; private set; } = null!;
    public decimal Price { get; private set; }

    private Product() { }

    public static Product Create(Guid id, string name, List<string> categories, string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(description);
        ArgumentException.ThrowIfNullOrEmpty(imageFile);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var product = new Product
        {
            Id = id,
            Name = name,
            Categories = categories,
            Description = description,
            ImageFile = imageFile,
            Price = price,
            CreatedAt = DateTime.UtcNow
        };

        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }

    public void Update(string name, List<string> categories, string description, string imageFile, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(description);
        ArgumentException.ThrowIfNullOrEmpty(imageFile);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        
        Name = name;
        Categories = categories;
        Description = description;
        ImageFile = imageFile;
        LastModified = DateTime.UtcNow;

        if(price != Price)
        {
            Price = price;
            AddDomainEvent(new ProductPriceChangedEvent(this));
        }
    }

}
