using System.Collections.Generic;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IWorkflowDefinitionBuilder
    {
        string? Id { get; }
        int Version { get; }
        IActivity? Root { get; }
        ICollection<ITrigger> Triggers { get; }
        IWorkflowDefinitionBuilder WithId(string id);
        IWorkflowDefinitionBuilder WithVersion(int version);
        IWorkflowDefinitionBuilder WithRoot(IActivity root);
        IWorkflowDefinitionBuilder AddTrigger(ITrigger trigger);
        WorkflowDefinition BuildWorkflow();
    }
}