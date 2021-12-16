using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.InMemory.Services;
using Elsa.Persistence.Requests;

namespace Elsa.Persistence.InMemory.Handlers.Requests;

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