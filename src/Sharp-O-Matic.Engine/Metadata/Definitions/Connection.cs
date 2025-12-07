namespace SharpOMatic.Engine.Metadata.Definitions;

public class Connection : ConnectionSummary
{
    public required string ConfigId { get; set; }
    public required string AuthenticationModeId { get; set; }
    public required Dictionary<string, string?> FieldValues { get; set; }
}
