using Basket.ShoppingCarts.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.JsonConvertors;

public class ShoppingCartConverter : JsonConverter<ShoppingCart>
{
    public override ShoppingCart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;

        var id = root.GetProperty(nameof(ShoppingCart.Id)).GetGuid();
        var userName = root.GetProperty(nameof(ShoppingCart.UserName)).GetString();
        var itemsElement = root.GetProperty(nameof(ShoppingCart.Items));

        var shoppingCart = ShoppingCart.Create(userName ?? string.Empty, id);

        var items = itemsElement.Deserialize<List<ShoppingCartItem>>(options);
        if (items != null) 
        {
            var itemsField = typeof(ShoppingCart).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            itemsField?.SetValue(shoppingCart, items);
        }

        return shoppingCart;
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(nameof(ShoppingCart.Id), value.Id);
        writer.WriteString(nameof(ShoppingCart.UserName), value.UserName);

        writer.WritePropertyName(nameof(ShoppingCart.Items));
        JsonSerializer.Serialize(writer, value.Items, options);

        writer.WriteEndObject();
    }
}
