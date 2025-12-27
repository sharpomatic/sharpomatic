namespace SharpOMatic.Editor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchemaTypeController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> GetSchemaTypeNames(ISchemaTypeRegistry schemaTypeRegistry)
    {
        return schemaTypeRegistry.GetTypeNames();
    }

    [HttpGet("{typeName}")]
    public string GetSchemaTypeNames(ISchemaTypeRegistry schemaTypeRegistry, string typeName)
    {
        return schemaTypeRegistry.GetSchema(typeName);
    }
}
