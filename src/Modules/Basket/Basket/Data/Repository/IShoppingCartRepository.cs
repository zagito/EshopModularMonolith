using Basket.ShoppingCarts.Models;

namespace Basket.Data.Repository;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetAsync(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default);
    Task<ShoppingCart> CreateAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userName, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(string? userName = null, CancellationToken token = default);
}
