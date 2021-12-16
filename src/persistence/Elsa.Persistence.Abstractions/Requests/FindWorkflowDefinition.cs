using Elsa.Mediator.Contracts;
using Elsa.Persistence.Entities;
using Elsa.Persistence.Models;

namespace Elsa.Persistence.Requests;

public record FindWorkflowDefinition(string DefinitionId, VersionOptions VersionOptions) : IRequest<WorkflowDefinition?>;