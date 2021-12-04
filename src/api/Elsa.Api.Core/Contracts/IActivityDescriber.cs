using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface IActivityDescriber
{
    ValueTask<ActivityDescriptor> DescribeActivityAsync(Type activityType, CancellationToken cancellationToken = default);
}