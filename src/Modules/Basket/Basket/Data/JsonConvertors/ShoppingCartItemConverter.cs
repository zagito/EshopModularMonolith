using Basket.ShoppingCarts.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.JsonConvertors;

public class ShoppingCartItemConverter : JsonConverter<ShoppingCartItem>
{
    public override ShoppingCartItem? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;

        var id = root.GetProperty(nameof(ShoppingCartItem.Id)).GetGuid();
        var shoppingCartId = root.GetProperty(nameof(ShoppingCartItem.ShoppingCartId)).GetGuid();
        var productId = root.GetProperty(nameof(ShoppingCartItem.ProductId)).GetGuid();
        var quantity = root.GetProperty(nameof(ShoppingCartItem.Quantity)).GetInt32();
        var color = root.GetProperty(nameof(ShoppingCartItem.Color)).GetString();
        var price = root.GetProperty(nameof(ShoppingCartItem.Price)).GetDecimal();
        var productName = root.GetProperty(nameof(ShoppingCartItem.ProductName)).GetString();

        return new ShoppingCartItem(id, shoppingCartId, productId, quantity, color ?? string.Empty, price, productName ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCartItem value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(nameof(ShoppingCartItem.Id), value.Id);
        writer.WriteString(nameof(ShoppingCartItem.ShoppingCartId), value.ShoppingCartId.ToString());
        writer.WriteString(nameof(ShoppingCartItem.ProductId), value.ProductId.ToString());
        writer.WriteNumber(nameof(ShoppingCartItem.Quantity), value.Quantity);
        writer.WriteString(nameof(ShoppingCartItem.Color), value.Color);
        writer.WriteNumber(nameof(ShoppingCartItem.Price), value.Price);
        writer.WriteString(nameof(ShoppingCartItem.ProductName), value.ProductName);

        writer.WriteEndObject();
    }
}
