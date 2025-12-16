using System.Text.Json;
using System.Text.Json.Schema;

namespace SharpOMatic.Engine.Helpers;

public static class SharpOMaticJsonUtilities
{
    public static JsonElement CreateJsonSchema(Type type)
    {
        var schema = JsonSchemaExporter.GetJsonSchemaAsNode(JsonSerializerOptions.Default, type);
        return JsonSerializer.SerializeToElement(schema, JsonSerializerOptions.Default);
    }
}
