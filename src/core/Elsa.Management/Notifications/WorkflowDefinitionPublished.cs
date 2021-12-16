using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;

namespace Elsa.Management.Notifications;
public record WorkflowDefinitionPublished(WorkflowDefinition WorkflowDefinition) : INotification;