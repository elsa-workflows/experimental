using System;
using System.Reflection;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Extensions
{
    public static class TypeSystemExtensions
    {
        public static TypeDescriptor Register<T>(this ITypeSystem typeSystem, string? typeName = default) where T : IActivity => typeSystem.Register(typeName ?? typeof(T).Name, typeof(T));

        public static TypeDescriptor Register(this ITypeSystem typeSystem, string typeName, Type type)
        {
            var kind = GetKind(type);

            if (kind == TypeKind.Unknown)
                throw new ArgumentException($"The specified type must implement either IActivity, ITrigger or both.");

            var descriptor = new TypeDescriptor(typeName, type, kind);
            typeSystem.Register(descriptor);
            return descriptor;
        }

        private static TypeKind GetKind(Type type)
        {
            var kind = TypeKind.Unknown;
            var isActivity = typeof(IActivity).IsAssignableFrom(type);
            var isTrigger = typeof(ITrigger).IsAssignableFrom(type);

            if (isActivity)
                kind |= TypeKind.Activity;

            if (isTrigger)
                kind |= TypeKind.Trigger;

            return kind;
        }
    }
}