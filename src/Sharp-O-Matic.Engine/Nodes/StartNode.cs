namespace SharpOMatic.Engine.Nodes;

[Node(NodeType.Start)]
public class StartNode(ThreadContext threadContext, StartNodeEntity node) : RunNode<StartNodeEntity>(threadContext, node)
{
    protected override async Task<(string, List<NextNodeData>)> RunInternal()
    {
        RunContext.Run.RunStatus = RunStatus.Running;
        RunContext.Run.Message = "Running";
        RunContext.Run.Started = DateTime.Now;
        await RunContext.RunUpdated();

        if (Node.ApplyInitialization)
        {
            var outputContext = new ContextObject();

            var provided = 0;
            var defaulted = 0;
            foreach (var entry in Node.Initializing.Entries)
            {
                if (string.IsNullOrWhiteSpace(entry.InputPath))
                    throw new SharpOMaticException($"Start node path cannot be empty.");

                if (ThreadContext.NodeContext.TryGet<object?>(entry.InputPath, out var mapValue))
                {
                    outputContext.TrySet(entry.InputPath, mapValue);
                    provided++;
                }
                else
                {
                    if (!entry.Optional)
                        throw new SharpOMaticException($"Start node mandatory path '{entry.InputPath}' cannot be resolved.");

                    var entryValue = await EvaluateContextEntryValue(entry);

                    if (!ThreadContext.NodeContext.TrySet(entry.InputPath, entryValue))
                        throw new SharpOMaticException($"Start node entry '{entry.InputPath}' could not be assigned the value.");

                    defaulted++;
                }
            }

            ThreadContext.NodeContext = outputContext;
            Trace.Message = $"{provided} provided, {defaulted} defaulted";
        }
        else
            Trace.Message = "Entered workflow";

        return (Trace.Message, [new NextNodeData(ThreadContext, RunContext.ResolveSingleOutput(Node))]);
    }
}
