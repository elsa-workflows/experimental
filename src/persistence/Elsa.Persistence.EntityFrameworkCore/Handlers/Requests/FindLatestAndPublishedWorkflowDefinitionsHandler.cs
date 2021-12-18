using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Requests;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Requests;

public class FindLatestAndPublishedWorkflowDefinitionsHandler : IRequestHandler<FindLatestAndPublishedWorkflowDefinitions, IEnumerable<WorkflowDefinition>>
{
    private readonly InMemoryStore<WorkflowDefinition> _store;

    public FindLatestAndPublishedWorkflowDefinitionsHandler(InMemoryStore<WorkflowDefinition> store)
    {
        _store = store;
    }

    public Task<IEnumerable<WorkflowDefinition>> HandleAsync(FindLatestAndPublishedWorkflowDefinitions request, CancellationToken cancellationToken)
    {
        var definitions = _store.FindMany(x => x.DefinitionId == request.DefinitionId && (x.IsLatest || x.IsPublished));
        return Task.FromResult(definitions);
    }
}