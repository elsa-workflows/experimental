using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.EntityFrameworkCore.Contracts;
using Elsa.Persistence.Extensions;
using Elsa.Persistence.Models;
using Elsa.Persistence.Requests;
using Microsoft.EntityFrameworkCore;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Requests;

public class ListWorkflowDefinitionsHandler : IRequestHandler<ListWorkflowDefinitions, PagedList<WorkflowDefinitionSummary>>
{
    private readonly IStore<WorkflowDefinition> _store;
    public ListWorkflowDefinitionsHandler(IStore<WorkflowDefinition> store) => _store = store;

    public async Task<PagedList<WorkflowDefinitionSummary>> HandleAsync(ListWorkflowDefinitions request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _store.CreateDbContextAsync(cancellationToken);
        var set = dbContext.WorkflowDefinitions;
        var query = set.AsQueryable();

        if (request.VersionOptions != null)
            query = query.WithVersion(request.VersionOptions.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var summaries = query.OrderBy(x => x.Name).Select(x => WorkflowDefinitionSummary.FromDefinition(x)).Skip(request.Skip).Take(request.Take).ToList();
        return new PagedList<WorkflowDefinitionSummary>(summaries, request.Take, totalCount);
    }
}