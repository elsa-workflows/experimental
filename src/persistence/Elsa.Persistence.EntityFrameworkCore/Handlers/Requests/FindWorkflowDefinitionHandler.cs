using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Requests;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Requests;

public class FindWorkflowDefinitionHandler : IRequestHandler<FindWorkflowDefinition, WorkflowDefinition?>
{
    private readonly InMemoryStore<WorkflowDefinition> _store;

    public FindWorkflowDefinitionHandler(InMemoryStore<WorkflowDefinition> store)
    {
        _store = store;
    }

    public Task<WorkflowDefinition?> HandleAsync(FindWorkflowDefinition request, CancellationToken cancellationToken)
    {
        var definition = _store.Find(x => x.DefinitionId == request.DefinitionId && x.WithVersion(request.VersionOptions));

        return Task.FromResult(definition);
    }
}