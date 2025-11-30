namespace SharpOMatic.Engine.Nodes;

public class FanOutNode(RunContext runContext, ContextObject nodeContext, FanOutNodeEntity node) : RunNode<FanOutNodeEntity>(runContext, nodeContext, node)
{
    protected override async Task<(string, List<NextNodeData>)> RunInternal()
    {
        var resolveNodes = RunContext.ResolveMultipleOutputs(Node);

        List<NextNodeData> nextNodes = [];
        foreach(var resolveNode in resolveNodes)
        {
            // TODO copy using serialization
            nextNodes.Add(new NextNodeData(NodeContext, resolveNode));
        }

        return ($"{nextNodes.Count} threads started", nextNodes);
    }
}
