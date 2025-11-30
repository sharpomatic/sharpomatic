namespace SharpOMatic.Engine.Nodes;

public abstract class RunNode<T> where T : NodeEntity
{
    protected RunContext RunContext { get; init; }
    protected ContextObject NodeContext { get; set; }
    protected T Node { get; init; }
    protected Trace Trace { get; init; }

    public RunNode(RunContext runContext, ContextObject nodeContext, NodeEntity node)
    {
        RunContext = runContext;
        NodeContext = nodeContext;
        Node = (T)node;

        Trace = new Trace()
        {
            WorkflowId = runContext.Workflow.Id,
            RunId = runContext.RunId,
            TraceId = Guid.NewGuid(),
            NodeEntityId = node.Id,
            Created = DateTime.Now,
            NodeType = node.NodeType,
            NodeStatus = NodeStatus.Running,
            Title = node.Title,
            Message = "Running",
            InputContext = RunContext.TypedSerialization(NodeContext)
        };
    }

    public async Task<List<NextNodeData>> Run()
    {
        await NodeRunning();

        try
        {
            (var message, var nextNodes) = await RunInternal();
            await NodeSuccess(message);
            return nextNodes;
        }
        catch (Exception ex)
        {
            await NodeFailed(ex.Message);
            throw;
        }
    }

    protected abstract Task<(string, List<NextNodeData>)> RunInternal();

    protected async Task NodeRunning()
    {
        await RunContext.Repository.UpsertTrace(Trace);
        await RunContext.Notifications.TraceProgress(Trace);
    }

    protected Task NodeSuccess(string message)
    {
        Trace.NodeStatus = NodeStatus.Success;
        return NodeUpdated(message);
    }

    protected Task NodeFailed(string exception)
    {
        Trace.NodeStatus = NodeStatus.Failed;
        Trace.Error = exception;
        return NodeUpdated("Failed");
    }

    protected async Task NodeUpdated(string message)
    {
        Trace.Finished = DateTime.Now;
        Trace.Message = message;
        Trace.OutputContext = RunContext.TypedSerialization(NodeContext);
        await RunContext.Repository.UpsertTrace(Trace);
        await RunContext.Notifications.TraceProgress(Trace);
    }

    protected Task<object?> EvaluateContextEntryValue(ContextEntryEntity entry)
    {
        return ContextHelpers.EvaluateContextEntryValue(NodeContext, entry);
    }
}
