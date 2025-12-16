namespace SharpOMatic.Engine.Services;

public class TypeSchemaService(IEnumerable<Type> types) : ITypeSchemaService
{
    private readonly Dictionary<string, Type> _types = types.ToDictionary(t => t.Name, t => t);

    public IEnumerable<string> GetTypeNames()
    {
        return _types.Keys;
    }

    public string GetSchema(string typeName)
    {
        if (!_types.TryGetValue(typeName, out var type))
        {
            throw new ArgumentException($"Type '{typeName}' not found.", nameof(typeName));
        }

        var schemaElement = SharpOMaticJsonUtilities.CreateJsonSchema(type);
        return JsonSerializer.Serialize(schemaElement);
    }
}
