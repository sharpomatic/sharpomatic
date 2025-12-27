namespace SharpOMatic.Engine.Interfaces;

public interface IEngineService
{
    Task<Guid> RunWorkflowAndNotify(Guid workflowId, ContextObject? context = null, ContextEntryListEntity? inputEntries = null);
    Task<Run> RunWorkflowAndWait(Guid workflowId, ContextObject? context = null, ContextEntryListEntity? inputEntries = null);
    Run RunWorkflowSynchronously(Guid workflowId, ContextObject? context = null, ContextEntryListEntity? inputEntries = null);
}
