using System.Net.NetworkInformation;

namespace SharpOMatic.Engine.Nodes;

public class EndNode(RunContext runContext, ContextObject nodeContext, EndNodeEntity node) : RunNode<EndNodeEntity>(runContext, nodeContext, node)
{
    protected override async Task<(string, List<NextNodeData>)> RunInternal()
    {
        if (Node.ApplyMappings)
        {
            var outputContext = new ContextObject();

            var mapped = 0;
            var missing = 0;
            foreach (var mapping in Node.Mappings.Entries)
            {
                if (string.IsNullOrWhiteSpace(mapping.InputPath))
                    throw new SharpOMaticException($"End node input path cannot be empty.");

                if (string.IsNullOrWhiteSpace(mapping.OutputPath))
                    throw new SharpOMaticException($"End node output path cannot be empty.");

                if (NodeContext.TryGet<object?>(mapping.InputPath, out var mapValue))
                {
                    outputContext.TrySet(mapping.OutputPath, mapValue);
                    mapped++;
                }
                else
                    missing++;
            }

            NodeContext = outputContext;
            Trace.Message = $"{mapped} mapped, {missing} missing";
        }
        else
            Trace.Message = "Exited workflow";

        // Last run EndNode has its output used as the output of the workflow
        RunContext.Run.OutputContext = RunContext.TypedSerialization(NodeContext);

        return (Trace.Message, []);
    }
}
