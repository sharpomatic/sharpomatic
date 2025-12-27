namespace SharpOMatic.Engine.Interfaces;

public interface INotificationService
{
    Task RunProgress(Run run);
    Task TraceProgress(Trace trace);
}
