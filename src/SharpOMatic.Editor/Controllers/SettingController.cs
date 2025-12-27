namespace SharpOMatic.Editor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingController : ControllerBase
{
    [HttpGet]
    public async Task<List<Setting>> GetSettings(IRepository repository)
    {
        return await repository.GetSettings();
    }

    [HttpPost]
    public async Task UpsertSetting(IRepository repository, [FromBody] Setting setting)
    {
        await repository.UpsertSetting(setting);
    }
}
