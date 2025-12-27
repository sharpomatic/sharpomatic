namespace SharpOMatic.Editor.Services;

public class NotificationService(IHubContext<NotificationHub> hubContext) : INotificationService
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
