using System;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Extensions
{
    public static class TriggerTypeRegistryExtensions
    {
        public static TriggerType Register<T>(this ITriggerTypeRegistry registry, string? typeName = default) where T : ITrigger => registry.Register(typeName ?? typeof(T).Name, typeof(T));

        public static TriggerType Register(this ITriggerTypeRegistry registry, string typeName, Type type)
        {
            var triggerType = new TriggerType(typeName, type);
            registry.Register(triggerType);
            return triggerType;
        }
    }
}