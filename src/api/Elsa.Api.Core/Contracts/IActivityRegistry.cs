using System;
using System.Collections.Generic;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface IActivityRegistry
{
    void Add(object provider, ActivityDescriptor descriptor);
    void AddMany(object provider, IEnumerable<ActivityDescriptor> descriptors);
    void Clear();
    void ClearProvider(object provider);
    IEnumerable<ActivityDescriptor> ListAll();
    IEnumerable<ActivityDescriptor> ListByProvider(object provider);
    ActivityDescriptor? Find(Func<ActivityDescriptor, bool> predicate);
    ActivityDescriptor? Find(string activityType);
}