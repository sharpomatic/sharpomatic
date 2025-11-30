namespace SharpOMatic.Engine.Nodes;

public class StartNode(RunContext runContext, ContextObject nodeContext, StartNodeEntity node) : RunNode<StartNodeEntity>(runContext, nodeContext, node)
{
    protected override async Task<(string, List<NextNodeData>)> RunInternal()
    {
        if (Node.ApplyInitialization)
        {
            var outputContext = new ContextObject();

            var provided = 0;
            var defaulted = 0;
            foreach (var entry in Node.Initializing.Entries)
            {
                if (string.IsNullOrWhiteSpace(entry.InputPath))
                    throw new SharpOMaticException($"Start node path cannot be empty.");

                if (NodeContext.TryGet<object?>(entry.InputPath, out var mapValue))
                {
                    outputContext.TrySet(entry.InputPath, mapValue);
                    provided++;
                }
                else
                {
                    if (!entry.Optional)
                        throw new SharpOMaticException($"Start node mandatory path '{entry.InputPath}' cannot be resolved.");

                    var entryValue = await EvaluateContextEntryValue(entry);

                    if (!NodeContext.TrySet(entry.InputPath, entryValue))
                        throw new SharpOMaticException($"Start node entry '{entry.InputPath}' could not be assigned the value.");

                    defaulted++;
                }
            }

            NodeContext = outputContext;
            Trace.Message = $"{provided} provided, {defaulted} defaulted";
        }
        else
            Trace.Message = "Entered workflow";

        return (Trace.Message, [new NextNodeData(NodeContext, RunContext.ResolveSingleOutput(Node))]);
    }
}
