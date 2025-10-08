using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Data.Repository;
public class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache)
    : IBasketRepository
{
    private readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
    };

    public async Task<ShoppingCart> CreateBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        await repository.CreateBasket(shoppingCart, cancellationToken);
        await cache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart), cancellationToken);
        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        
        if(!asNoTracking)
            return await repository.GetBasket(userName, asNoTracking, cancellationToken);

        if (cachedBasket is not null)
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, options)!;
        
        var basket = await repository.GetBasket(userName, asNoTracking, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        
        return basket;
    }

    public async Task<int> SaveChanges(string? userName = null, CancellationToken cancellationToken = default)
    {
        int result = await repository.SaveChanges(userName, cancellationToken);
        
        if (userName is not null)
        {
            var basket = await repository.GetBasket(userName, false, cancellationToken);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket, options), cancellationToken);
        }

        return result;
    }

}
