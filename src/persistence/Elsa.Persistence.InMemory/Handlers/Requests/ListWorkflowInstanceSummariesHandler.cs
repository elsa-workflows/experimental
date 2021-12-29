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

public class ListWorkflowInstanceSummariesHandler : IRequestHandler<ListWorkflowInstanceSummaries, PagedList<WorkflowInstanceSummary>>
{
    private readonly InMemoryStore<WorkflowInstance> _store;
    public ListWorkflowInstanceSummariesHandler(InMemoryStore<WorkflowInstance> store) => _store = store;

    public Task<PagedList<WorkflowInstanceSummary>> HandleAsync(ListWorkflowInstanceSummaries request, CancellationToken cancellationToken)
    {
        var query = _store.List();
        var totalCount = query.Count();
        var entities = query.Skip(request.Skip).Take(request.Take).ToList();
        var summaries = entities.Select(WorkflowInstanceSummary.FromInstance).ToList();
        var pagedList = new PagedList<WorkflowInstanceSummary>(summaries, request.Take, totalCount);
        return Task.FromResult(pagedList);
    }
}