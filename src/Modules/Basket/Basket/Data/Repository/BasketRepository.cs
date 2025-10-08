namespace Basket.Data.Repository;
public class BasketRepository(BasketDbContext context)
    : IBasketRepository
{
    public async Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        context.ShoppingCarts.Add(shoppingCart);
        await context.SaveChangesAsync(cancellationToken);
        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        var shoppingCart = await GetBasket(userName, false, cancellationToken);

        context.ShoppingCarts.Remove(shoppingCart);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        =>  (asNoTracking
            ? await context.ShoppingCarts
            .Include(sc => sc.Items)
            .AsNoTracking()
            .SingleOrDefaultAsync(sc => sc.UserName == userName, cancellationToken)
            : await context.ShoppingCarts
            .Include(sc => sc.Items)
            .SingleOrDefaultAsync(sc => sc.UserName == userName, cancellationToken)) 
            ?? throw new BasketNotFoundException(userName);

    public Task<int> SaveChanges(string? userName = null, CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);

    
}
