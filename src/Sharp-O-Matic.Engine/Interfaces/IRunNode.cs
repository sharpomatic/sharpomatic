namespace SharpOMatic.Engine.Interfaces;

public interface IRunNode
{
    Task<List<NextNodeData>> Run();
}
