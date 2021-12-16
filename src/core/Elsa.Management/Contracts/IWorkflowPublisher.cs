using Elsa.Persistence.Entities;

namespace Elsa.Management.Contracts
{
    public interface IWorkflowPublisher
    {
        WorkflowDefinition New();
        Task<WorkflowDefinition?> PublishAsync(string definitionId, CancellationToken cancellationToken = default);
        Task<WorkflowDefinition> PublishAsync(WorkflowDefinition definition, CancellationToken cancellationToken = default);
        Task<WorkflowDefinition?> RetractAsync(string definitionId, CancellationToken cancellationToken = default);
        Task<WorkflowDefinition> RetractAsync(WorkflowDefinition definitionId, CancellationToken cancellationToken = default);
        Task<WorkflowDefinition?> GetDraftAsync(string definitionId, CancellationToken cancellationToken= default);
        Task<WorkflowDefinition> SaveDraftAsync(WorkflowDefinition definition, CancellationToken cancellationToken = default);
        Task DeleteAsync(string definitionId, CancellationToken cancellationToken = default);
        Task DeleteAsync(WorkflowDefinition definitionId, CancellationToken cancellationToken = default);
    }
}