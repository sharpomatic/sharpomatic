namespace SharpOMatic.Engine.Interfaces;

public interface IScriptOptionsService
{
    IReadOnlyCollection<Assembly> GetAssemblies();
    IReadOnlyCollection<string> GetImports();
    ScriptOptions GetScriptOptions();
}
