namespace SharpOMatic.Engine.Services;

public class NodeExecutionService(INodeQueue queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var (runContext, contextObject, node) = await queue.DequeueAsync(stoppingToken);

                // If the workflow has already been failed, then ignore the node execution
                if (runContext.Run.RunStatus != RunStatus.Failed)
                    await ProcessNode(runContext, contextObject, node);
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
                break;
            }
        }
    }

    private async Task ProcessNode(RunContext runContext, ContextObject nodeContext, NodeEntity node)
    {
        try
        {
            var nextNodes = await EngineService.RunNode(runContext, nodeContext, node);

            if (runContext.UpdateRunningThreadCount(nextNodes.Count - 1) == 0)
            {
                runContext.Run.RunStatus = RunStatus.Success;
                runContext.Run.Message = "Success";
                runContext.Run.Stopped = DateTime.Now;

                // If no EndNode was encountered then use the output of the last run node
                if (runContext.Run.OutputContext is null)
                    runContext.Run.OutputContext = runContext.TypedSerialization(nodeContext);

                await runContext.RunUpdated();
            }
            else
            {
                foreach (var nextNode in nextNodes)
                    queue.Enqueue(runContext, nextNode.NodeContext, nextNode.Node);
            }
        }
        catch (Exception ex)
        {
            runContext.Run.RunStatus = RunStatus.Failed;
            runContext.Run.Message = "Failed";
            runContext.Run.Error = ex.Message;
            runContext.Run.Stopped = DateTime.Now;

            // If no EndNode was encountered then use the output of the last run node
            if (runContext.Run.OutputContext is null)
                runContext.Run.OutputContext = runContext.TypedSerialization(nodeContext);

            await runContext.RunUpdated();
        }
    }
}
