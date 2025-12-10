namespace SharpOMatic.Engine.Nodes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NodeAttribute(NodeType nodeType) : Attribute
{
    public NodeType NodeType { get; } = nodeType;
}
