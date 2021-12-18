using Elsa.Mediator.Contracts;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Commands;

public class SaveWorkflowDefinitionHandler : ICommandHandler<SaveWorkflowDefinition>
{
    private readonly InMemoryStore<WorkflowDefinition> _store;

    public SaveWorkflowDefinitionHandler(InMemoryStore<WorkflowDefinition> store)
    {
        _store = store;
    }

    public Task<Unit> HandleAsync(SaveWorkflowDefinition command, CancellationToken cancellationToken)
    {
        _store.Save(command.WorkflowDefinition.Id, command.WorkflowDefinition);
        
        return Task.FromResult(Unit.Instance);
    }
}