namespace SharpOMatic.Engine.Entities.Definitions;

public class EndNodeEntity : NodeEntity
{
    public required bool ApplyMappings { get; set; }
    public required ContextEntryListEntity Mappings { get; set; }
}

