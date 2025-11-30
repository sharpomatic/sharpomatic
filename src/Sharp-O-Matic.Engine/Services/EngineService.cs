namespace SharpOMatic.Engine.Services;

public class EngineService(IRepository Repository,
                           INodeQueue Queue,
                           INotification Notifications,
                           IEnumerable<JsonConverter> JsonConverters) : IEngine
{


    public async Task<Guid> RunWorkflow(Guid workflowId, ContextObject? context = null, ContextEntryListEntity? inputEntries = null)
    {
        context ??= [];

        string? inputJson = null;
        if (inputEntries is not null)
        {
            inputJson = JsonSerializer.Serialize(inputEntries);

            foreach (var entry in inputEntries!.Entries)
            {
                var entryValue = await ContextHelpers.EvaluateContextEntryValue(context, entry);
                if (!context.TrySet(entry.InputPath, entryValue))
                    throw new SharpOMaticException($"Input entry '{entry.InputPath}' could not be assigned the value.");
            }
        }

        var run = new Run()
        {
            WorkflowId = workflowId,
            RunId = Guid.NewGuid(),
            RunStatus = RunStatus.Created,
            Message = "Created",
            Created = DateTime.Now,
            InputEntries = inputJson,
        };

        await RunUpdated(run);

        var workflow = await Repository.GetWorkflow(workflowId) ?? throw new SharpOMaticException($"Could not load workflow {workflowId}.");
        var currentNodes = workflow.Nodes.Where(n => n.NodeType == NodeType.Start).ToList();
        if (currentNodes.Count != 1)
            throw new SharpOMaticException("Must have exactly one start node.");

        var runContext = new RunContext(Repository, Notifications, JsonConverters, workflow, run.RunId);

        run.RunStatus = RunStatus.Running;
        run.Message = "Running";
        run.Started = DateTime.Now;
        run.InputContext = runContext.TypedSerialization(context);
        await RunUpdated(run);

        Queue.Enqueue(runContext, context, currentNodes[0]);

        return run.RunId;
    }

    public static Task<List<NextNodeData>> RunNode(RunContext runContext, ContextObject nodeContext, NodeEntity node)
    {
        return node switch
        {
            StartNodeEntity startNode => new StartNode(runContext, nodeContext, startNode).Run(),
            EndNodeEntity endNode => new EndNode(runContext, nodeContext, endNode).Run(),
            EditNodeEntity editNode => new EditNode(runContext, nodeContext, editNode).Run(),
            CodeNodeEntity codeNode => new CodeNode(runContext, nodeContext, codeNode).Run(),
            SwitchNodeEntity switchNode => new SwitchNode(runContext, nodeContext, switchNode).Run(),
            FanInNodeEntity fanInNode => new FanInNode(runContext, nodeContext, fanInNode).Run(),
            FanOutNodeEntity fanOutNode => new FanOutNode(runContext, nodeContext, fanOutNode).Run(),
            _ => throw new SharpOMaticException($"Unrecognized node type' {node.NodeType}'")
        };
    }

    private async Task RunUpdated(Run run)
    {
        await Repository.UpsertRun(run);
        await Notifications.RunProgress(run);
    }
}
