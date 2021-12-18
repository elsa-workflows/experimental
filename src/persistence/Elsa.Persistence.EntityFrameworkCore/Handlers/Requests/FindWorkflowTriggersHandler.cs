using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Requests;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Requests;

public class FindWorkflowTriggersHandler : IRequestHandler<FindWorkflowTriggers, IEnumerable<WorkflowTrigger>>
{
    private readonly InMemoryStore<WorkflowTrigger> _store;
    public FindWorkflowTriggersHandler(InMemoryStore<WorkflowTrigger> store) => _store = store;

    public Task<IEnumerable<WorkflowTrigger>> HandleAsync(FindWorkflowTriggers request, CancellationToken cancellationToken)
    {
        var triggers = _store.FindMany(x => x.Name == request.Name && x.Hash == request.Hash).ToList().AsEnumerable();
        return Task.FromResult(triggers);
    }
}