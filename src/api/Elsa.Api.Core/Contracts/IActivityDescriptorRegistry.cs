using System;
using System.Collections.Generic;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts
{
    public interface IActivityDescriptorRegistry
    {
        void AddDescriptor(ActivityDescriptor activityDescriptor);
        void AddDescriptors(IEnumerable<ActivityDescriptor> activityDescriptors);
        void Clear();
        IEnumerable<ActivityDescriptor> ListDescriptors();
        ActivityDescriptor? FindDescriptor(Func<ActivityDescriptor> predicate);
        ActivityDescriptor? FindDescriptor(string activityType);
    }
}