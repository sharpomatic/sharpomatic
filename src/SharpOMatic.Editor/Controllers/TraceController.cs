namespace SharpOMatic.Editor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TraceController : ControllerBase
{
    [HttpGet("forrun/{id}")]
    public async Task<IEnumerable<Trace>> GetRunTraces(IRepository repository, Guid id)
    {
        return await repository.GetRunTraces(id);
    }
}
