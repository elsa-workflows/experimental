using Elsa.Mediator.Contracts;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Commands;

public class SaveWorkflowBookmarksHandler : ICommandHandler<SaveWorkflowBookmarks>
{
    private readonly InMemoryStore<WorkflowBookmark> _store;

    public SaveWorkflowBookmarksHandler(InMemoryStore<WorkflowBookmark> store)
    {
        _store = store;
    }

    public Task<Unit> HandleAsync(SaveWorkflowBookmarks command, CancellationToken cancellationToken)
    {
        _store.SaveMany(command.WorkflowBookmarks, x => x.Id);

        return Task.FromResult(Unit.Instance);
    }
}