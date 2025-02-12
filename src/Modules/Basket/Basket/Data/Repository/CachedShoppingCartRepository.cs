using Basket.ShoppingCarts.Models;
using Microsoft.Extensions.Caching.Hybrid;

namespace Basket.Data.Repository;

public class CachedShoppingCartRepository(IShoppingCartRepository repository,
    HybridCache cache) : IShoppingCartRepository
{
    public async Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(GetCacheKey(shoppingCart.UserName), cancellationToken);

        return await cache.GetOrCreateAsync(GetCacheKey(shoppingCart.UserName), async token =>
        {
            return await repository.CreateAsync(shoppingCart, token);
        },
        cancellationToken: cancellationToken);
    }

    public async Task<bool> DeleteAsync(string userName, CancellationToken cancellationToken = default)
    {
        bool isDeleted = await repository.DeleteAsync(userName, cancellationToken);
        if (isDeleted)
            await cache.RemoveAsync(GetCacheKey(userName), cancellationToken);

        return isDeleted;
    }

    public async Task<ShoppingCart?> GetAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
            return await repository.GetAsync(userName, asNoTracking, cancellationToken);

        return await cache.GetOrCreateAsync(GetCacheKey(userName), async token =>
        {
            return await repository.GetAsync(userName, asNoTracking, token);
        }, cancellationToken: cancellationToken);
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken token = default)
    {
        var result = await repository.SaveChangesAsync(userName, token);

        if (userName is not null)
            await cache.RemoveAsync(GetCacheKey(userName), token);

        return result;
    }

    private static string GetCacheKey(string userName) => $"ShoppingCart-{userName}";
}
