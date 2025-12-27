namespace SharpOMatic.Engine.Services;

public class EngineService(INodeQueueService QueueService,
                           IRepositoryService RepositoryService,
                           IRunContextFactory RunContextFactory,
                           IScriptOptionsService ScriptOptionsService,
                           IJsonConverterService JsonConverterService) : IEngineService
{
    public async Task<Guid> RunWorkflow(Guid workflowId, ContextObject? nodeContext = null, ContextEntryListEntity? inputEntries = null)
    {
        nodeContext ??= [];

        string? inputJson = null;
        if (inputEntries is not null)
        {
            inputJson = JsonSerializer.Serialize(inputEntries);

            foreach (var entry in inputEntries!.Entries)
            {
                var entryValue = await ContextHelpers.ResolveContextEntryValue(nodeContext, entry, ScriptOptionsService);
                if (!nodeContext.TrySet(entry.InputPath, entryValue))
                    throw new SharpOMaticException($"Input entry '{entry.InputPath}' could not be assigned the value.");
            }
        }

        var workflow = await RepositoryService.GetWorkflow(workflowId) ?? throw new SharpOMaticException($"Could not load workflow {workflowId}.");
        var currentNodes = workflow.Nodes.Where(n => n.NodeType == NodeType.Start).ToList();
        if (currentNodes.Count != 1)
            throw new SharpOMaticException("Must have exactly one start node.");

        var converters = JsonConverterService.GetConverters();

        var run = new Run()
        {
            WorkflowId = workflowId,
            RunId = Guid.NewGuid(),
            RunStatus = RunStatus.Created,
            Message = "Created",
            Created = DateTime.Now,
            InputEntries = inputJson,
            InputContext = JsonSerializer.Serialize(nodeContext, new JsonSerializerOptions().BuildOptions(converters))
        };

        var nodeRunLimitSetting = await RepositoryService.GetSetting("RunNodeLimit");
        var nodeRunLimit = nodeRunLimitSetting?.ValueInteger ?? NodeExecutionService.DEFAULT_NODE_RUN_LIMIT;

        var runContext = RunContextFactory.Create(workflow, run, converters, nodeRunLimit);
        var threadContext = new ThreadContext(runContext, nodeContext);

        await runContext.RunUpdated();

        QueueService.Enqueue(threadContext, currentNodes[0]);
        return run.RunId;
    }
}
