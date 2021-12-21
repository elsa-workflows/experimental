using System.Linq.Expressions;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.EntityFrameworkCore.Contracts;
using Elsa.Persistence.Extensions;
using Elsa.Persistence.Requests;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Requests;

public class FindWorkflowDefinitionHandler : IRequestHandler<FindWorkflowDefinition, WorkflowDefinition?>
{
    private readonly IStore<WorkflowDefinition> _store;
    public FindWorkflowDefinitionHandler(IStore<WorkflowDefinition> store) => _store = store;

    public async Task<WorkflowDefinition?> HandleAsync(FindWorkflowDefinition request, CancellationToken cancellationToken)
    {
        Expression<Func<WorkflowDefinition, bool>> predicate = x => x.DefinitionId == request.DefinitionId;
        predicate = predicate.WithVersion(request.VersionOptions);
        return await _store.FindAsync(predicate, cancellationToken);
    }
}