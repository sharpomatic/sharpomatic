namespace SharpOMatic.Editor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetadataController(IRepositoryService repositoryService) : ControllerBase
{
    [HttpGet("connector-configs")]
    public async Task<IEnumerable<ConnectorConfig>> GetConnectorConfigs()
    {
        return await repositoryService.GetConnectorConfigs();
    }

    [HttpGet("connectors")]
    public Task<List<ConnectorSummary>> GetConnectorSummaries(IRepositoryService repositoryService)
    {
        return repositoryService.GetConnectorSummaries();
    }

    [HttpGet("connectors/{id}")]
    public async Task<ActionResult<Connector>> GetConnector(IRepositoryService repositoryService, Guid id)
    {
        return await repositoryService.GetConnector(id);
    }

    [HttpPost("connectors")]
    public async Task UpsertConnector(IRepositoryService repositoryService, [FromBody]Connector connector)
    {
        await repositoryService.UpsertConnector(connector);
    }

    [HttpDelete("connectors/{id}")]
    public async Task DeleteConnector(IRepositoryService repositoryService, Guid id)
    {
        await repositoryService.DeleteConnector(id);
    }

    [HttpGet("model-configs")]
    public async Task<IEnumerable<ModelConfig>> GetModelConfigs()
    {
        return await repositoryService.GetModelConfigs();
    }

    [HttpGet("models")]
    public Task<List<ModelSummary>> GetModelSummaries(IRepositoryService repositoryService)
    {
        return repositoryService.GetModelSummaries();
    }

    [HttpGet("models/{id}")]
    public async Task<ActionResult<Model>> GetModel(IRepositoryService repositoryService, Guid id)
    {
        return await repositoryService.GetModel(id);
    }

    [HttpPost("models")]
    public async Task UpsertModel(IRepositoryService repositoryService, [FromBody] Model model)
    {
        await repositoryService.UpsertModel(model);
    }

    [HttpDelete("models/{id}")]
    public async Task DeleteModel(IRepositoryService repositoryService, Guid id)
    {
        await repositoryService.DeleteModel(id);
    }
}
