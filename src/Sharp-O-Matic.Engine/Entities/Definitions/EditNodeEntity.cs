namespace SharpOMatic.Engine.Entities.Definitions;

public class EditNodeEntity : NodeEntity
{
    public required ContextEntryListEntity Edits { get; set; }
}

