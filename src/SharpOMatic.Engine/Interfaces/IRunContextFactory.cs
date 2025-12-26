namespace SharpOMatic.Engine.Interfaces;

public interface IRunContextFactory
{
    RunContext Create(
        WorkflowEntity workflow,
        Run run,
        IEnumerable<JsonConverter> jsonConverters,
        int runNodeLimit);
}
