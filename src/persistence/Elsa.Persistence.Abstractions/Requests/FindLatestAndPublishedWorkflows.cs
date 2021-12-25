using System.Collections.Generic;
using Elsa.Mediator.Contracts;
using Elsa.Models;
using Elsa.Persistence.Entities;

namespace Elsa.Persistence.Requests;

public record FindLatestAndPublishedWorkflows(string DefinitionId) : IRequest<IEnumerable<Workflow>>;