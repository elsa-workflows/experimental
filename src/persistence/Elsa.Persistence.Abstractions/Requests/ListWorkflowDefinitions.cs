using Elsa.Mediator.Contracts;
using Elsa.Persistence.Models;

namespace Elsa.Persistence.Requests;

public record ListWorkflowDefinitions(VersionOptions? VersionOptions = default, int Skip = 0, int Take = 50) : IRequest<PagedList<WorkflowDefinitionSummary>>;