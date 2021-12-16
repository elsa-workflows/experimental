using System.Collections.Generic;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;

namespace Elsa.Persistence.Requests;

public record FindLatestAndPublishedWorkflowDefinitions(string DefinitionId) : IRequest<IEnumerable<WorkflowDefinition>>;