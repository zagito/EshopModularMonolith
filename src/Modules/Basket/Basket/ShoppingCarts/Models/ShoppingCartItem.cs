using Basket.Data.JsonConvertors;
using System.Text.Json.Serialization;

namespace Basket.ShoppingCarts.Models;

[JsonConverter(typeof(ShoppingCartItemConverter))]
public class ShoppingCartItem : Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; } = default!;
    public Guid ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
    public string ProductName { get; private set; } = default!;
    //private ShoppingCartItem() { }

    internal ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
    {
        ArgumentException.ThrowIfNullOrEmpty(color);
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductName = productName;
    }

    internal ShoppingCartItem(Guid id, Guid shoppingCartId, Guid productId, int quantity, string color, decimal price, string productName)
        : this(shoppingCartId, productId, quantity, color, price, productName)
    {
        Id = id;
    }

    internal void AddQuantity(int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(quantity);
        Quantity += quantity;
    }

    internal void UpdatePrice(decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        Price = price;
    }
}
