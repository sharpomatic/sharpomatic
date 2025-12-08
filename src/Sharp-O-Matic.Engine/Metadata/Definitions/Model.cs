namespace SharpOMatic.Engine.Metadata.Definitions;

public class Model : ModelSummary
{
    public required string ConfigId { get; set; }
    public required Guid? ConnectionId { get; set; }
    public required ModelCapabilities CustomCapabilities { get; set; }
    public required Dictionary<string, string?> ParameterValues { get; set; }
}

