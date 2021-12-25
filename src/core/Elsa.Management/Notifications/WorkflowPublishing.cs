using Elsa.Mediator.Contracts;
using Elsa.Models;
using Elsa.Persistence.Entities;

namespace Elsa.Management.Notifications;

public record WorkflowPublishing(Workflow Workflow) : INotification;