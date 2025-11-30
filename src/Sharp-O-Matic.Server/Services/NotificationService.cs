using SharpOMatic.Engine.Interfaces;

namespace SharpOMatic.Server.Services;

public class NotificationService(IHubContext<NotificationHub> hubContext) : INotification
{
    public async Task RunProgress(Run model)
    {
        await hubContext.Clients.All.SendAsync("RunProgress", model);
    }

    public async Task TraceProgress(Trace model)
    {
        await hubContext.Clients.All.SendAsync("TraceProgress", model);
    }
}
