namespace SharpOMatic.Engine.Interfaces;

public interface IRunNodeFactory
{
    IRunNode Create(ThreadContext threadContext, NodeEntity node);
}
