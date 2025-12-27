namespace SharpOMatic.Engine.Interfaces;

public interface IEngineService
{
    Task<Guid> RunWorkflow(Guid workflowId, ContextObject? context = null, ContextEntryListEntity? inputEntries = null);
}
