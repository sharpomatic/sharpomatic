namespace SharpOMatic.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetadataController(IRepository repository) : ControllerBase
{
    [HttpGet("connection-configs")]
    public async Task<IEnumerable<ConnectionConfig>> GetConnectionConfigs()
    {
        return await repository.GetConnectionConfigs();
    }

    [HttpGet("connections")]
    public Task<List<ConnectionSummary>> GetConnectionSummaries(IRepository repository)
    {
        return (from c in repository.GetConnections()
                orderby c.Name
                select new ConnectionSummary()
                {
                    ConnectionId = c.ConnectionId,
                    Name = c.Name,
                    Description = c.Description,
                }).ToListAsync();
    }

    [HttpGet("connections/{id}")]
    public async Task<ActionResult<Connection>> GetConnection(IRepository repository, Guid id)
    {
        return await repository.GetConnection(id);
    }

    [HttpPost("connections")]
    public async Task UpsertConnection(IRepository repository, [FromBody]Connection connection)
    {
        await repository.UpsertConnection(connection);
    }

    [HttpDelete("connections/{id}")]
    public async Task DeleteConnection(IRepository repository, Guid id)
    {
        await repository.DeleteConnection(id);
    }

    [HttpGet("model-configs")]
    public async Task<IEnumerable<ModelConfig>> GetModelConfigs()
    {
        return await repository.GetModelConfigs();
    }

    [HttpGet("models")]
    public Task<List<ModelSummary>> GetModelSummaries(IRepository repository)
    {
        return (from m in repository.GetModels()
                orderby m.Name
                select new ModelSummary()
                {
                    ModelId = m.ModelId,
                    Name = m.Name,
                    Description = m.Description,
                }).ToListAsync();
    }

    [HttpGet("models/{id}")]
    public async Task<ActionResult<Model>> GetModel(IRepository repository, Guid id)
    {
        return await repository.GetModel(id);
    }

    [HttpPost("models")]
    public async Task UpsertModel(IRepository repository, [FromBody] Model model)
    {
        await repository.UpsertModel(model);
    }

    [HttpDelete("models/{id}")]
    public async Task DeleteModel(IRepository repository, Guid id)
    {
        await repository.DeleteModel(id);
    }
}
