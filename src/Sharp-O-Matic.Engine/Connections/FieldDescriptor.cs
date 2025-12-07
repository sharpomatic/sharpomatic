
namespace SharpOMatic.Engine.Connections;

public class FieldDescriptor
{
    public required string Name { get; set; }
    public required string Label { get; set; }
    public required string Description { get; set; }
    public required FieldDescriptorType Type { get; set; }
    public required bool IsRequired { get; set; }
    public object? DefaultValue { get; set; }
    public List<string>? EnumOptions { get; set; }
    public double? Min { get; set; }
    public double? Max { get; set; }
    public double? Step { get; set; }
}