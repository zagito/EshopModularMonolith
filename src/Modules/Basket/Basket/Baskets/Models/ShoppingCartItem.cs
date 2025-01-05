namespace Basket.Baskets.Models;

public class ShoppingCartItem : Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; } = default!;
    public Guid ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
    public string ProductName { get; private set; } = default!;
    private ShoppingCartItem() { }

    internal static ShoppingCartItem Create(Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
    {
        ArgumentException.ThrowIfNullOrEmpty(color);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        return new ShoppingCartItem
        {
            Id = Guid.NewGuid(),
            ShoppingCartId = shoppingCartId,
            ProductId = productId,
            Quantity = quantity,
            Color = color,
            Price = price,
            ProductName = productName
        };
    }

    internal void AddQuantity(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        Quantity += quantity;
    }
}
