namespace SharpOMatic.Engine.Interfaces;

public interface IRepository
{
    // ------------------------------------------------
    // Workflow Operations
    // ------------------------------------------------
    IQueryable<Workflow> GetWorkflows();
    Task<WorkflowEntity> GetWorkflow(Guid workflowId);
    Task UpsertWorkflow(WorkflowEntity workflow);
    Task DeleteWorkflow(Guid workflowId);

    // ------------------------------------------------
    // Run Operations
    // ------------------------------------------------
    IQueryable<Run> GetRuns();
    IQueryable<Run> GetWorkflowRuns(Guid workflowId);
    Task UpsertRun(Run run);

    // ------------------------------------------------
    // Trace Operations
    // ------------------------------------------------
    IQueryable<Trace> GetTraces();
    IQueryable<Trace> GetRunTraces(Guid runId);
    Task UpsertTrace(Trace trace);

    // ------------------------------------------------
    // ConnectionConfig Operations
    // ------------------------------------------------
    Task<List<ConnectionConfig>> GetConnectionConfigs();
    Task<ConnectionConfig?> GetConnectionConfig(string configId);
    Task UpsertConnectionConfig(ConnectionConfig config);

    // ------------------------------------------------
    // Connection Operations
    // ------------------------------------------------
    IQueryable<ConnectionMetadata> GetConnections();
    Task<Connection> GetConnection(Guid connectionId);
    Task UpsertConnection(Connection connection);
    Task DeleteConnection(Guid connectionId);

    // ------------------------------------------------
    // ModelConfig Operations
    // ------------------------------------------------
    Task<List<ModelConfig>> GetModelConfigs();
    Task<ModelConfig?> GetModelConfig(string configId);
    Task UpsertModelConfig(ModelConfig config);

    // ------------------------------------------------
    // Model Operations
    // ------------------------------------------------
    IQueryable<ModelMetadata> GetModels();
    Task<Model> GetModel(Guid modelId);
    Task UpsertModel(Model model);
    Task DeleteModel(Guid modelId);
}
