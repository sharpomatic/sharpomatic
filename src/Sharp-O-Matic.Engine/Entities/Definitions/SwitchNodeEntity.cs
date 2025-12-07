namespace SharpOMatic.Engine.Entities.Definitions;

public class SwitchNodeEntity : NodeEntity
{
    public required SwitchEntryEntity[] Switches { get; set; }
}

