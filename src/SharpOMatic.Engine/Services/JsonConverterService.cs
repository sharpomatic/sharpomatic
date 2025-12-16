using System.Text.Json.Serialization;

namespace SharpOMatic.Engine.Services;

public class JsonConverterService : IJsonConverterService
{
    private readonly List<JsonConverter> _converters = [];

    public JsonConverterService(IEnumerable<Type> converterTypes)
    {
        foreach (var ct in converterTypes)
        {
            if (!typeof(JsonConverter).IsAssignableFrom(ct))
                throw new ArgumentException($"Type '{ct.FullName}' is not a JsonConverter.");

            if (Activator.CreateInstance(ct) is not JsonConverter converterInstance)
                throw new ArgumentException($"Could not create instance of '{ct.FullName}'.");

            _converters.Add(converterInstance);
        }
    }

    public IEnumerable<JsonConverter> GetConverters()
    {
        return _converters;
    }
}
