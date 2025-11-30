namespace SharpOMatic.Engine.Services;

public class NodeQueueService : INodeQueue
{
    private readonly Channel<(RunContext runContext, ContextObject nodeContext, NodeEntity node)> _queue;

    public NodeQueueService()
    {
        _queue = Channel.CreateUnbounded<(RunContext, ContextObject, NodeEntity)>();
    }

    public void Enqueue(RunContext runContext, ContextObject nodeContext, NodeEntity node)
    {
        _queue.Writer.TryWrite((runContext, nodeContext, node));
    }

    public ValueTask<(RunContext runContext, ContextObject nodeContext, NodeEntity node)> DequeueAsync(CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAsync(cancellationToken);
    }
}
