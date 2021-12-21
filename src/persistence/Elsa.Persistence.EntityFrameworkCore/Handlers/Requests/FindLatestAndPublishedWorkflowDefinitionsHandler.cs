using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.EntityFrameworkCore.Contracts;
using Elsa.Persistence.Requests;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Requests;

public class FindLatestAndPublishedWorkflowDefinitionsHandler : IRequestHandler<FindLatestAndPublishedWorkflowDefinitions, IEnumerable<WorkflowDefinition>>
{
    private readonly IStore<WorkflowDefinition> _store;
    public FindLatestAndPublishedWorkflowDefinitionsHandler(IStore<WorkflowDefinition> store) => _store = store;

    public async Task<IEnumerable<WorkflowDefinition>> HandleAsync(FindLatestAndPublishedWorkflowDefinitions request, CancellationToken cancellationToken) =>
        await _store.FindManyAsync(x => x.DefinitionId == request.DefinitionId && (x.IsLatest || x.IsPublished), cancellationToken);
}