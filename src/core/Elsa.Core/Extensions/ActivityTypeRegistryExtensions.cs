using System;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Extensions
{
    public static class ActivityTypeRegistryExtensions
    {
        public static ActivityType Register<T>(this IActivityTypeRegistry registry, string? typeName = default) where T : IActivity => registry.Register(typeName ?? typeof(T).Name, typeof(T));

        public static ActivityType Register(this IActivityTypeRegistry registry, string typeName, Type type)
        {
            var activityType = new ActivityType(typeName, type);
            registry.Register(activityType);
            return activityType;
        }
    }
}