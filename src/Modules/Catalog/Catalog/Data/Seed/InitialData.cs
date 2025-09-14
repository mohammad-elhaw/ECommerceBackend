namespace Catalog.Data.Seed;
public static class InitialData
{
    public static List<Product> Products =>
        new List<Product>
        {
            Product.Create(Guid.NewGuid(), "IPhone X", ["category1"], "Long description", "imageFile", 500),
            Product.Create(Guid.NewGuid(), "Samsung 10", ["category1"], "Long description", "imageFile", 300),
            Product.Create(Guid.NewGuid(), "Huawei Plus", ["category2"], "Long description", "imageFile", 600),
            Product.Create(Guid.NewGuid(), "Oppo Reno 6", ["category2"], "Long description", "imageFile", 700)
        };
}
