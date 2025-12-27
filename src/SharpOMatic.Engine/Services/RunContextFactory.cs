namespace SharpOMatic.Engine.Services;

public class RunContextFactory(IServiceScopeFactory scopeFactory) : IRunContextFactory
{
    public RunContext Create(
        WorkflowEntity workflow,
        Run run,
        IEnumerable<JsonConverter> jsonConverters,
        int runNodeLimit)
    {
        ArgumentNullException.ThrowIfNull(workflow);
        ArgumentNullException.ThrowIfNull(run);
        ArgumentNullException.ThrowIfNull(jsonConverters);

        var scope = scopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        var notifications = scope.ServiceProvider.GetRequiredService<INotification>();
        var toolMethodRegistry = scope.ServiceProvider.GetRequiredService<IToolMethodRegistry>();
        var schemaTypeService = scope.ServiceProvider.GetRequiredService<ISchemaTypeService>();
        var scriptOptionsService = scope.ServiceProvider.GetRequiredService<IScriptOptionsService>();

        return new RunContext(
            scope,
            repository,
            notifications,
            toolMethodRegistry,
            schemaTypeService,
            scriptOptionsService,
            jsonConverters,
            workflow,
            run,
            runNodeLimit);
    }
}
