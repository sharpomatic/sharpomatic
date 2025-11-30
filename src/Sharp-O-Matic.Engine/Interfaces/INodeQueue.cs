namespace SharpOMatic.Engine.Interfaces;

public interface INodeQueue
{
    void Enqueue(RunContext runContext, ContextObject nodeContext, NodeEntity node);
    ValueTask<(RunContext runContext, ContextObject nodeContext, NodeEntity node)> DequeueAsync(CancellationToken cancellationToken);
}
