using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface ITriggerProvider
{
    ValueTask<IEnumerable<TriggerDescriptor>> GetDescriptorsAsync(CancellationToken cancellationToken = default);
}