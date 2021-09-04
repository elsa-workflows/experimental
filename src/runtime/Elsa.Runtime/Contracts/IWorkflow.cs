namespace Elsa.Runtime.Contracts
{
    public interface IWorkflow
    {
        string Id { get; }
        int Version { get; }
        void Build(IWorkflowBuilder builder);
    }
}