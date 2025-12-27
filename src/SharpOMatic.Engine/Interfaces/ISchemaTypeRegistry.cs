namespace SharpOMatic.Engine.Interfaces;

public interface ISchemaTypeRegistry
{
    IEnumerable<string> GetTypeNames();
    string GetSchema(string typeName);
}
