namespace SharpOMatic.Editor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToolController : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> GetToolDisplayNames(IToolMethodRegistry toolMethodRegistry)
    {
        return toolMethodRegistry.GetToolDisplayNames();
    }
}
