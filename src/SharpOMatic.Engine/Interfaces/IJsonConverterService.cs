using System.Text.Json.Serialization;

namespace SharpOMatic.Engine.Interfaces;

public interface IJsonConverterService
{
    IEnumerable<JsonConverter> GetConverters();
}
