using Elsa.Mediator.Contracts;
using Elsa.Persistence.Abstractions.Entities;

namespace Elsa.Persistence.Abstractions.Commands;

public record SaveWorkflowDefinition(WorkflowDefinition WorkflowDefinition) : ICommand;