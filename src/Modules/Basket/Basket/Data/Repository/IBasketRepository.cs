namespace Basket.Data.Repository;
public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true,
        CancellationToken cancellationToken = default);
    Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    Task<int> SaveChanges(string? userName = null, CancellationToken cancellationToken = default);
}
