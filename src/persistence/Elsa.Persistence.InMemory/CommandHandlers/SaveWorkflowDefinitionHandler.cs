using System.Threading.Tasks;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Abstractions.Commands;

namespace Elsa.Persistence.InMemory.CommandHandlers;

public class SaveWorkflowDefinitionHandler : ICommandHandler<SaveWorkflowDefinition>
{
    public Task<Unit> HandleAsync(SaveWorkflowDefinition command)
    {
        return Task.FromResult(Unit.Instance);
    }
}