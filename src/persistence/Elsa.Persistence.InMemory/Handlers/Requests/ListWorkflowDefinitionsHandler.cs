using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Extensions;
using Elsa.Persistence.InMemory.Services;
using Elsa.Persistence.Models;
using Elsa.Persistence.Requests;

// ReSharper disable PossibleMultipleEnumeration

namespace Elsa.Persistence.InMemory.Handlers.Requests;

public class ListWorkflowDefinitionsHandler : IRequestHandler<ListWorkflowSummaries, PagedList<WorkflowSummary>>
{
    private readonly InMemoryStore<WorkflowDefinition> _store;
    public ListWorkflowDefinitionsHandler(InMemoryStore<WorkflowDefinition> store) => _store = store;

    public Task<PagedList<WorkflowSummary>> HandleAsync(ListWorkflowSummaries request, CancellationToken cancellationToken)
    {
        var query = _store.List();

        if (request.VersionOptions != null)
            query = query.WithVersion(request.VersionOptions.Value);

        var totalCount = query.Count();
        var entities = query.Skip(request.Skip).Take(request.Take).ToList();
        var summaries = entities.Select(WorkflowSummary.FromDefinition).ToList();
        var pagedList = new PagedList<WorkflowSummary>(summaries, request.Take, totalCount);
        return Task.FromResult(pagedList);
    }
}