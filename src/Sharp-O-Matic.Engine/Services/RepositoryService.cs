namespace SharpOMatic.Engine.Services;

public class RepositoryService(IDbContextFactory<SharpOMaticDbContext> dbContextFactory) : IRepository
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new NodeEntityConverter() }
    };

    public IQueryable<Workflow> GetWorkflows()
    {
        var dbContext = dbContextFactory.CreateDbContext();
        return dbContext.Workflows;
    }

    public async Task<WorkflowEntity> GetWorkflow(Guid workflowId)
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        var workflow = await (from w in dbContext.Workflows
                              where w.WorkflowId == workflowId
                              select w).AsNoTracking().FirstOrDefaultAsync();

        return workflow is null
            ? throw new SharpOMaticException($"Workflow '{workflowId}' cannot be found.")
            : new WorkflowEntity()
            {
                Id = workflow.WorkflowId,
                Name = workflow.Named,
                Description = workflow.Description,
                Nodes = JsonSerializer.Deserialize<NodeEntity[]>(workflow.Nodes, _options)!,
                Connections = JsonSerializer.Deserialize<ConnectionEntity[]>(workflow.Connections, _options)!,
            };
    }

    public async Task UpsertWorkflow(WorkflowEntity workflow)
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        var entry = await (from w in dbContext.Workflows
                           where w.WorkflowId == workflow.Id
                           select w).FirstOrDefaultAsync();

        if (entry is null)
        {
            entry = new Workflow()
            {
                WorkflowId = workflow.Id,
                Named = "",
                Description = "",
                Nodes = "",
                Connections = ""
            };

            dbContext.Workflows.Add(entry);
        }

        entry.Named = workflow.Name;
        entry.Description = workflow.Description;
        entry.Nodes = JsonSerializer.Serialize(workflow.Nodes, _options);
        entry.Connections = JsonSerializer.Serialize(workflow.Connections, _options);

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteWorkflow(Guid workflowId)
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        var workflow = await (from w in dbContext.Workflows
                              where w.WorkflowId == workflowId
                              select w).FirstOrDefaultAsync();

        if (workflow is null)
            throw new SharpOMaticException($"Workflow '{workflowId}' cannot be found.");

        dbContext.Remove(workflow);
        await dbContext.SaveChangesAsync();
    }

    public IQueryable<Run> GetRuns()
    {
        var dbContext = dbContextFactory.CreateDbContext();
        return dbContext.Runs;
    }

    public IQueryable<Run> GetWorkflowRuns(Guid workflowId)
    {
        var dbContext = dbContextFactory.CreateDbContext();
        return (from r in dbContext.Runs
                where r.WorkflowId == workflowId
                select r);
    }

    public async Task UpsertRun(Run run)
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        var entity = await (from r in dbContext.Runs
                            where r.RunId == run.RunId
                            select r).FirstOrDefaultAsync();

        if (entity is null)
            dbContext.Runs.Add(run);
        else
            dbContext.Entry(entity).CurrentValues.SetValues(run);

        await dbContext.SaveChangesAsync();
    }

    public IQueryable<Trace> GetTraces()
    {
        var dbContext = dbContextFactory.CreateDbContext();
        return dbContext.Traces;
    }

    public IQueryable<Trace> GetRunTraces(Guid runId)
    {
        var dbContext = dbContextFactory.CreateDbContext();

        return (from t in dbContext.Traces
                where t.RunId == runId
                select t);
    }

    public async Task UpsertTrace(Trace trace)
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        var entity = await (from t in dbContext.Traces
                            where t.TraceId == trace.TraceId
                            select t).FirstOrDefaultAsync();

        if (entity is null)
            dbContext.Traces.Add(trace);
        else
            dbContext.Entry(entity).CurrentValues.SetValues(trace);

        await dbContext.SaveChangesAsync();
    }

    public async Task<ConnectionConfig?> GetConnectionConfig(string id)
    {
        var dbContext = dbContextFactory.CreateDbContext();

        var entry = await (from c in dbContext.ConnectionMetadata
                           where c.Id == id
                           select c).AsNoTracking().FirstOrDefaultAsync();

        if (entry is null)
            return null;

        return JsonSerializer.Deserialize<ConnectionConfig>(entry.Config, _options);

    }

    public async Task UpsertConnectionConfig(ConnectionConfig config)
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        var entry = await (from c in dbContext.ConnectionMetadata
                           where c.Id == config.Id
                           select c).FirstOrDefaultAsync();

        if (entry is null)
        {
            entry = new ConnectionMetadata()
            {
                Id = config.Id,
                Config = ""
            };

            dbContext.ConnectionMetadata.Add(entry);
        }

        entry.Config = JsonSerializer.Serialize(config, _options);
        await dbContext.SaveChangesAsync();
    }
}
