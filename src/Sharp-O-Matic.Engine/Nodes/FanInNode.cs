namespace SharpOMatic.Engine.Nodes;

public class FanInNode(RunContext runContext, ContextObject nodeContext, FanInNodeEntity node) : RunNode<FanInNodeEntity>(runContext, nodeContext, node)
{
    protected override async Task<(string, List<NextNodeData>)> RunInternal()
    {
        return ("Threads synchronized", [new NextNodeData(NodeContext, RunContext.ResolveSingleOutput(Node))]);
    }
}
