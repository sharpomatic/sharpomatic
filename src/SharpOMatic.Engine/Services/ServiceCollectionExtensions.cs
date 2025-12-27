namespace SharpOMatic.Engine.Services;

public static class ServiceCollectionExtensions
{
    public static SharpOMaticBuilder AddSharpOMaticEngine(this IServiceCollection services, Action<SharpOMaticOptions>? optionsAction = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<SharpOMaticOptions>();
        if (optionsAction is not null)
            services.Configure(optionsAction);

        // Add mandatory services
        services.TryAddSingleton<ICodeCheck, CodeCheckService>();
        services.TryAddSingleton<INodeQueue, NodeQueueService>();
        services.TryAddSingleton<IRunNodeFactory, RunNodeFactory>();
        services.TryAddSingleton<IRunContextFactory, RunContextFactory>();
        services.TryAddScoped<IRepository, RepositoryService>();
        services.TryAddScoped<IEngine, EngineService>();
        services.AddHostedService<NodeExecutionService>();

        // Add empty versions of optional services
        services.TryAddSingleton<ISchemaTypeService>(_ => new SchemaTypeService([]));
        services.TryAddSingleton<IToolMethodRegistry>(_ => new ToolMethodRegistry([]));
        services.TryAddSingleton<IScriptOptionsService>(_ => new ScriptOptionsService([], []));
        services.TryAddSingleton<IJsonConverterService>(_ => new JsonConverterService([]));

        return new SharpOMaticBuilder(services);
    }
}
