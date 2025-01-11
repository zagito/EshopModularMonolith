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

        var id = root.GetProperty("id").GetGuid();
        var shoppingCartId = root.GetProperty("shoppingCartId").GetGuid();
        var productId = root.GetProperty("productId").GetGuid();
        var quantity = root.GetProperty("quantity").GetInt32();
        var color = root.GetProperty("color").GetString();
        var price = root.GetProperty("price").GetDecimal();
        var productName = root.GetProperty("productName").GetString();

        return new ShoppingCartItem(id, shoppingCartId, productId, quantity, color ?? string.Empty, price, productName ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCartItem value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", value.Id);
        writer.WriteString("shoppingCartId", value.ShoppingCartId.ToString());
        writer.WriteString("productId", value.ProductId.ToString());
        writer.WriteNumber("quantity", value.Quantity);
        writer.WriteString("color", value.Color);
        writer.WriteNumber("price", value.Price);
        writer.WriteString("productName", value.ProductName);

        writer.WriteEndObject();
    }
}
