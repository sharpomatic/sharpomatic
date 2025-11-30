namespace SharpOMatic.Engine.Nodes;

public class EditNode(RunContext runContext, ContextObject nodeContext, EditNodeEntity node) : RunNode<EditNodeEntity>(runContext, nodeContext, node)
{
    protected override async Task<(string, List<NextNodeData>)> RunInternal()
    {
        foreach (var entry in Node.Edits.Entries)
        {
            if (entry.Purpose == ContextEntryPurpose.Upsert)
            {
                if (string.IsNullOrWhiteSpace(entry.InputPath))
                    throw new SharpOMaticException($"Edit node upsert path cannot be empty.");

                var entryValue = await EvaluateContextEntryValue(entry);

                if (!NodeContext.TrySet(entry.InputPath, entryValue))
                    throw new SharpOMaticException($"Edit node entry '{entry.InputPath}' could not be assigned the value.");
            }
        }

        foreach (var entry in Node.Edits.Entries)
        {
            if (entry.Purpose == ContextEntryPurpose.Delete)
            {
                if (string.IsNullOrWhiteSpace(entry.InputPath))
                    throw new SharpOMaticException($"Edit node delete path cannot be empty.");

                NodeContext.RemovePath(entry.InputPath);
            }
        }

        var numUpserts = Node.Edits.Entries.Where(e => e.Purpose == ContextEntryPurpose.Upsert).Count();
        var numDeletes = Node.Edits.Entries.Where(e => e.Purpose == ContextEntryPurpose.Delete).Count();

        StringBuilder message = new();
        if (numUpserts == 0)
            message.Append("No upserts");
        else if (numUpserts == 1)
            message.Append("1 upsert");
        else
            message.Append($"{numUpserts} upserts");

        message.Append($", {numDeletes} deleted");

        return (message.ToString(), [new NextNodeData(NodeContext, RunContext.ResolveSingleOutput(Node))]);
    }
}
