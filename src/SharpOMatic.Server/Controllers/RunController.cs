namespace SharpOMatic.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RunController : ControllerBase
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [HttpGet("latestforworkflow/{id}")]
    public async Task<Run?> GetLatestRunForWorkflow(IRepository repository, Guid id)
    {
        var run = await (from r in repository.GetWorkflowRuns(id)
                         orderby r.Created descending
                         select r).FirstOrDefaultAsync();

        if (run is not null)
            NormalizeInputEntriesForClient(run);

        return run;
    }

    [HttpGet("latestforworkflow/{id}/{count}")]
    public async Task<List<Run>> GetLatestRunsForWorkflow(IRepository repository, Guid id, int count)
    {
        if (count < 1)
            return [];

        var runs = await (from r in repository.GetWorkflowRuns(id)
                          orderby r.Created descending
                          select r).Take(count).ToListAsync();

        foreach (var run in runs)
            NormalizeInputEntriesForClient(run);

        return runs;
    }

    private static void NormalizeInputEntriesForClient(Run run)
    {
        if (run.InputEntries is null)
            return;

        // Convert to camelCase which is used by the front end
        var entries = JsonSerializer.Deserialize<ContextEntryListEntity>(run.InputEntries);
        run.InputEntries = JsonSerializer.Serialize(entries, _options);
    }
}
