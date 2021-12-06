using System;
using System.Collections.Generic;
using Elsa.Api.Core.Models;

namespace Elsa.Api.Core.Contracts;

public interface ITriggerRegistry
{
    void Add(object provider, TriggerDescriptor descriptor);
    void AddMany(object provider, IEnumerable<TriggerDescriptor> descriptors);
    void Clear();
    void ClearProvider(object provider);
    IEnumerable<TriggerDescriptor> ListAll();
    IEnumerable<TriggerDescriptor> ListByProvider(object provider);
    TriggerDescriptor? Find(Func<TriggerDescriptor, bool> predicate);
    TriggerDescriptor? Find(string triggerType);
}