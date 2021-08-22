using System.Collections.Generic;
using System.Threading;
using Elsa.Runtime.Models;

namespace Elsa.Runtime.Contracts
{
    /// <summary>
    /// Represents a source of workflows.
    /// </summary>
    public interface IWorkflowProvider
    {
        IEnumerable<Workflow> GetWorkflowsAsync(CancellationToken cancellationToken = default);
    }
}