using System;

namespace Elsa.Helpers;

public static class TypeNameHelper
{
    private const string DefaultActivityNamespace = "Elsa.Activities.";
    private const string DefaultTriggerNamespace = "Elsa.Triggers.";

    public static string? GenerateActivityTypeNamespace(Type activityType) => GenerateTypeNamespace(activityType, DefaultActivityNamespace);
    public static string? GenerateTriggerTypeNamespace(Type triggerType) => GenerateTypeNamespace(triggerType, DefaultTriggerNamespace);

    public static string GenerateTypeName(Type type, string? ns)
    {
        var typeName = type.Name;
        return ns != null ? $"{ns}.{typeName}" : typeName;
    }

    public static string GenerateActivityTypeName(Type type)
    {
        var ns = GenerateActivityTypeNamespace(type);
        return GenerateTypeName(type, ns);
    }

    public static string GenerateTriggerTypeName(Type type)
    {
        var ns = GenerateTriggerTypeNamespace(type);
        return GenerateTypeName(type, ns);
    }

    public static string? GetCategoryFromNamespace(string? ns)
    {
        if (string.IsNullOrWhiteSpace(ns))
            return null;

        var index = ns.LastIndexOf('.');

        return index < 0 ? ns : ns[(index + 1)..];
    }

    private static string? GenerateTypeNamespace(Type type, string defaultNamespace) =>
        type.Namespace != null
            ? type.Namespace.StartsWith(defaultNamespace)
                ? type.Namespace[defaultNamespace.Length..]
                : type.Namespace
            : null;
}