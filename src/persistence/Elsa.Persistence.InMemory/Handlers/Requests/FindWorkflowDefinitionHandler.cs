using System.Threading;
using System.Threading.Tasks;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Extensions;
using Elsa.Persistence.InMemory.Services;
using Elsa.Persistence.Requests;

namespace Elsa.Persistence.InMemory.Handlers.Requests;

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