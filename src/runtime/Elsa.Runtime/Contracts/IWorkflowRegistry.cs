using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts;

public interface IWorkflowRegistry
{
    Task<Workflow?> FindByIdAsync(string id, VersionOptions versionOptions, CancellationToken cancellationToken = default);
    IAsyncEnumerable<Workflow> StreamAllAsync(CancellationToken cancellationToken = default);
}