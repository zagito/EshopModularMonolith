using Basket.ShoppingCarts.Models;

namespace Basket.Data.Repository;

public class ShoppingCartRepository(BasketDbContext dbContext) : IShoppingCartRepository
{
    public async Task<ShoppingCart?> GetAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = dbContext.ShoppingCarts.Include(x => x.Items)
            .Where(x => x.UserName == userName);

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        dbContext.ShoppingCarts.Add(shoppingCart);
        await dbContext.SaveChangesAsync(cancellationToken);
        return shoppingCart;
    }

    public async Task<bool> DeleteAsync(string userName, CancellationToken token = default)
    {
        var basket = await GetAsync(userName, false, token);
        if (basket is null) return false;

        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(token);
        return true;
    }

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken token = default)
    {
        return await dbContext.SaveChangesAsync(token);
    }
}
