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
                await ProcessNode(runContext, contextObject, node);
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
                break;
            }
            catch (Exception)
            {
            }
        }
    }

    private async Task ProcessNode(RunContext runContext, ContextObject nodeContext, NodeEntity node)
    {
        try
        {
            var nextNodes = await EngineService.RunNode(runContext, nodeContext, node);
            foreach (var nextNode in nextNodes)
                queue.Enqueue(runContext, nextNode.NodeContext, nextNode.Node);
        }
        catch
        {
        }
    }
}
