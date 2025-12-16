namespace SharpOMatic.Engine.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharpOMaticJsonConverters(this IServiceCollection services, IEnumerable<Type> converterTypes)
    {
        services.AddSingleton<IJsonConverterService>(new JsonConverterService(converterTypes));
        return services;
    }

    public static IServiceCollection AddSharpOMaticJsonConverters(this IServiceCollection services, params Type[] converterTypes)
        => services.AddSharpOMaticJsonConverters((IEnumerable<Type>)converterTypes);

    public static IServiceCollection AddSharpOMaticTypes(this IServiceCollection services, IEnumerable<Type> types)
    {
        services.AddSingleton<ITypeSchemaService>(new TypeSchemaService(types));
        return services;
    }

    public static IServiceCollection AddSharpOMaticTypes(this IServiceCollection services, params Type[] types)
        => services.AddSharpOMaticTypes((IEnumerable<Type>)types);

    public static IServiceCollection AddSharpOMaticEngine(this IServiceCollection services)
    {
        services.AddSingleton<ICodeCheck, CodeCheckService>();
        services.AddTransient<IRepository, RepositoryService>();
        services.AddTransient<IEngine, EngineService>();
        services.AddSingleton<INodeQueue, NodeQueueService>();
        services.AddHostedService<NodeExecutionService>();
        return services;
    }

    public static IServiceCollection AddSharpOMaticRepository(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction,
        Action<SharpOMaticDbOptions>? dbOptionsAction = null)
    {
        services.AddDbContextFactory<SharpOMaticDbContext>(optionsAction);
        
        if (dbOptionsAction != null)
            services.Configure(dbOptionsAction);
        else
            services.Configure<SharpOMaticDbOptions>(o => { });

        return services;
    }
}
