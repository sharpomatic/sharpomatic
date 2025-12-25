using SharpOMatic.Engine.Enumerations;

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

    [HttpGet("latestforworkflow/{id}/{page}/{count}")]
    public async Task<WorkflowRunPageResult> GetLatestRunsForWorkflow(
        IRepository repository,
        Guid id,
        int page,
        int count,
        [FromQuery] RunSortField sortBy = RunSortField.Created,
        [FromQuery] SortDirection sortDirection = SortDirection.Descending)
    {
        var totalCount = await repository.GetWorkflowRuns(id).CountAsync();
        if (count < 1 || page < 1 || totalCount == 0)
            return new WorkflowRunPageResult([], totalCount);

        var skip = (page - 1) * count;
        var sortedRuns = GetSortedRuns(repository.GetWorkflowRuns(id), sortBy, sortDirection);
        var runs = await sortedRuns.Skip(skip).Take(count).ToListAsync();

        foreach (var run in runs)
            NormalizeInputEntriesForClient(run);

        return new WorkflowRunPageResult(runs, totalCount);
    }

    private static IQueryable<Run> GetSortedRuns(IQueryable<Run> runs, RunSortField sortBy, SortDirection sortDirection)
    {
        return sortBy switch
        {
            RunSortField.Status => sortDirection == SortDirection.Ascending
                ? runs.OrderBy(r => r.RunStatus).ThenByDescending(r => r.Created)
                : runs.OrderByDescending(r => r.RunStatus).ThenByDescending(r => r.Created),
            _ => sortDirection == SortDirection.Ascending
                ? runs.OrderBy(r => r.Created)
                : runs.OrderByDescending(r => r.Created),
        };
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
