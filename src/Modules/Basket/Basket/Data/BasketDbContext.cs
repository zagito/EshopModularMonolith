using Basket.ShoppingCarts.Models;

namespace Basket.Data;

public class BasketDbContext(DbContextOptions<BasketDbContext> options) 
    : DbContext(options)
{
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();

    override protected void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("basket");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
