using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface ITriggerDescriber
{
    ValueTask<TriggerDescriptor> DescribeTriggerAsync(Type triggerType, CancellationToken cancellationToken = default);
}