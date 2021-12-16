using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;

namespace Elsa.Persistence.Commands;

/// <summary>
/// Represents a command to persist the specified <see cref="WorkflowDefinition"/> to storage.
/// </summary>
/// <param name="WorkflowDefinition"></param>
public record SaveWorkflowDefinition(WorkflowDefinition WorkflowDefinition) : ICommand;