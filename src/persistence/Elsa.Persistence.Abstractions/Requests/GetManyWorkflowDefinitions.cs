using System.Collections.Generic;
using Elsa.Mediator.Contracts;
using Elsa.Persistence.Models;

namespace Elsa.Persistence.Requests;

public record GetManyWorkflowDefinitions(string[] DefinitionIds, VersionOptions? VersionOptions = default) : IRequest<IEnumerable<WorkflowDefinitionSummary>>;