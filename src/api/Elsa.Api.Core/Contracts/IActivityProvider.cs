using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface IActivityProvider
{
    ValueTask<IEnumerable<ActivityDescriptor>> GetDescriptorsAsync(CancellationToken cancellationToken = default);
}