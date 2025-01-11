using Basket.Data.JsonConvertors;
using System.Text.Json.Serialization;

namespace Basket.ShoppingCarts.Models;

[JsonConverter(typeof(ShoppingCartConverter))]
public class ShoppingCart : Aggregate<Guid>
{
    public string UserName { get; private set; } = default!;
    private readonly List<ShoppingCartItem> _items = [];
    public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => _items.Sum(x => x.Price * x.Quantity); 
    private ShoppingCart() { }

    public static ShoppingCart Create(string userName)
    {
        return Create(userName, Guid.NewGuid());
    }

    public static ShoppingCart Create(string userName, Guid id)
    {
        ArgumentException.ThrowIfNullOrEmpty(userName);
        return new ShoppingCart
        {
            Id = id,
            UserName = userName
        };
    }

    public void AddItem(Guid productId, string productName, decimal price, int quantity, string color)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId && x.Color == color);
        if (existingItem != null)
        {
            existingItem.AddQuantity(quantity);
        }
        else
        {
            _items.Add(new ShoppingCartItem(Id, productId, quantity, color, price, productName));
        }
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
        }
    }
}
