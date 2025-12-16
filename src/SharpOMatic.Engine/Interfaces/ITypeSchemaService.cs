namespace SharpOMatic.Engine.Interfaces;

public interface ITypeSchemaService
{
    IEnumerable<string> GetTypeNames();
    string GetSchema(string typeName);
}
