namespace SharpOMatic.Engine.Nodes;

public class CodeNode(RunContext runContext, ContextObject nodeContext, CodeNodeEntity node) : RunNode<CodeNodeEntity>(runContext, nodeContext, node)
{
    protected override async Task<(string, List<NextNodeData>)> RunInternal()
    {
        if (!string.IsNullOrWhiteSpace(Node.Code))
        {
            var options = ScriptOptions.Default
                                .WithImports("System", "System.Threading.Tasks", "SharpOMatic.Engine.Contexts")
                                .WithReferences(typeof(Task).Assembly, typeof(ContextObject).Assembly);

            try
            {
                var result = await CSharpScript.RunAsync(Node.Code, options, new ScriptCodeContext() { Context = NodeContext }, typeof(ScriptCodeContext));
            }
            catch (CompilationErrorException e1)
            {
                // Return the first 3 errors only
                StringBuilder sb = new();
                sb.AppendLine($"Code node failed compilation.\n");
                foreach (var diagnostic in e1.Diagnostics.Take(3))
                    sb.AppendLine(diagnostic.ToString());

                throw new SharpOMaticException(sb.ToString());
            }
            catch (InvalidOperationException e2)
            {
                StringBuilder sb = new();
                sb.AppendLine($"Code node failed during execution.\n");
                sb.Append(e2.Message);
                throw new SharpOMaticException(sb.ToString());
            }
        }

        return ("Code executed", [new NextNodeData(NodeContext, RunContext.ResolveSingleOutput(Node))]);
    }
}
