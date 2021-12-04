using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts;

public interface ITrigger : INode
{
    ValueTask<IEnumerable<object>> GetHashInputsAsync(TriggerIndexingContext context, CancellationToken cancellationToken = default);
}