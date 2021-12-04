using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts;

/// <summary>
/// Represents a source of workflows.
/// </summary>
public interface IWorkflowProvider
{
    ValueTask<Workflow?> FindByIdAsync(string id, VersionOptions versionOptions, CancellationToken cancellationToken = default);
    IAsyncEnumerable<Workflow> StreamAllAsync(CancellationToken cancellationToken = default);
}