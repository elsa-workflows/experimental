using System.Threading;
using System.Threading.Tasks;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;
using Elsa.Persistence.InMemory.Services;

namespace Elsa.Persistence.InMemory.Handlers.Commands;

public class DeleteWorkflowBookmarksHandler : ICommandHandler<DeleteWorkflowBookmarks>
{
    private readonly InMemoryStore<WorkflowBookmark> _store;

    public DeleteWorkflowBookmarksHandler(InMemoryStore<WorkflowBookmark> store)
    {
        _store = store;
    }

    public Task<Unit> HandleAsync(DeleteWorkflowBookmarks command, CancellationToken cancellationToken)
    {
        _store.DeleteMany(command.BookmarkIds);
        
        return Task.FromResult(Unit.Instance);
    }
}