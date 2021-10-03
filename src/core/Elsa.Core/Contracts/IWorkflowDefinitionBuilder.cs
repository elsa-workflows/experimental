using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IWorkflowDefinitionBuilder
    {
        IWorkflowDefinitionBuilder WithId(string id);
        IWorkflowDefinitionBuilder WithVersion(int version);
        IWorkflowDefinitionBuilder WithRoot(IActivity root);
        IWorkflowDefinitionBuilder AddTrigger(ITrigger trigger);
        WorkflowDefinition BuildWorkflow();
    }
}