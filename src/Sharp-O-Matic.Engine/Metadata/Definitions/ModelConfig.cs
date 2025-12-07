namespace SharpOMatic.Engine.Metadata.Definitions;

public class ModelConfig
{
    public required string ConfigId { get; set; }
    public required string DisplayName { get; set; }
    public required string Description { get; set; }
    public required string ConnectionConfigId { get; set; }
    public required bool IsCustom { get; set; }
    public required ModelCapabilities Capabilities { get; set; }
    public required List<FieldDescriptor> ParameterFields { get; set; }
}

