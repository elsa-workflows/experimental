using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.EntityFrameworkCore.Contracts;
using Elsa.Persistence.Extensions;
using Elsa.Persistence.Models;
using Elsa.Persistence.Requests;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Requests;

public class ListWorkflowInstanceSummariesHandler : IRequestHandler<ListWorkflowInstanceSummaries, PagedList<WorkflowInstanceSummary>>
{
    private readonly IStore<WorkflowInstance> _store;
    public ListWorkflowInstanceSummariesHandler(IStore<WorkflowInstance> store) => _store = store;

    public async Task<PagedList<WorkflowInstanceSummary>> HandleAsync(ListWorkflowInstanceSummaries request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _store.CreateDbContextAsync(cancellationToken);
        var set = dbContext.WorkflowInstances;
        var query = set.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);
        var summaries = query.OrderBy(x => x.CreatedAt).Select(x => WorkflowInstanceSummary.FromInstance(x)).Skip(request.Skip).Take(request.Take).ToList();
        return new PagedList<WorkflowInstanceSummary>(summaries, request.Take, totalCount);
    }
}