using Elsa.Mediator.Contracts;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Commands;

public class SaveWorkflowInstanceHandler : ICommandHandler<SaveWorkflowInstance>
{
    private readonly InMemoryStore<WorkflowInstance> _store;

    public SaveWorkflowInstanceHandler(InMemoryStore<WorkflowInstance> store)
    {
        _store = store;
    }

    public Task<Unit> HandleAsync(SaveWorkflowInstance command, CancellationToken cancellationToken)
    {
        _store.Save(command.WorkflowInstance.Id, command.WorkflowInstance);
        
        return Task.FromResult(Unit.Instance);
    }
}