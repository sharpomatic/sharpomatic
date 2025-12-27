using SharpOMatic.Engine.Interfaces;

namespace SharpOMatic.Engine.Services;

public class EngineService(INodeQueueService QueueService,
                           IRepositoryService RepositoryService,
                           IRunContextFactory RunContextFactory,
                           IScriptOptionsService ScriptOptionsService,
                           IJsonConverterService JsonConverterService) : IEngineService
{
    public async Task<Guid> RunWorkflowAndNotify(Guid workflowId, ContextObject? nodeContext = null, ContextEntryListEntity? inputEntries = null)
    {
        var runContext = await CreateRunContextAndQueue(workflowId, nodeContext, inputEntries, null);
        return runContext.Run.RunId;
    }

    public async Task<Run> RunWorkflowAndWait(
        Guid workflowId,
        ContextObject? nodeContext = null,
        ContextEntryListEntity? inputEntries = null)
    {
        var completionSource = new TaskCompletionSource<Run>(TaskCreationOptions.RunContinuationsAsynchronously);
        await CreateRunContextAndQueue(workflowId, nodeContext, inputEntries, completionSource);
        return await completionSource.Task.ConfigureAwait(false);
    }

    public Run RunWorkflowSynchronously(Guid workflowId, ContextObject? context = null, ContextEntryListEntity? inputEntries = null)
    {
        return RunWorkflowAndWait(workflowId, context, inputEntries).GetAwaiter().GetResult();
    }

    private async Task<RunContext> CreateRunContextAndQueue(
        Guid workflowId,
        ContextObject? nodeContext,
        ContextEntryListEntity? inputEntries,
        TaskCompletionSource<Run>? completionSource)
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

        var runContext = RunContextFactory.Create(workflow, run, converters, nodeRunLimit, completionSource);
        var threadContext = new ThreadContext(runContext, nodeContext);

        await runContext.RunUpdated();

        QueueService.Enqueue(threadContext, currentNodes[0]);

        return runContext;
    }
}
