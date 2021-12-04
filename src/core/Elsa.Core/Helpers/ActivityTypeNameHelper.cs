using System;

namespace Elsa.Helpers;

public static class ActivityTypeNameHelper
{
    private const string DefaultActivityNamespace = "Elsa.Activities."; 
    
    public static string? GenerateActivityTypeNamespace(Type activityType) =>
        activityType.Namespace != null
            ? activityType.Namespace.StartsWith(DefaultActivityNamespace)
                ? activityType.Namespace[DefaultActivityNamespace.Length..]
                : activityType.Namespace
            : null;

    public static string GenerateActivityTypeName(Type activityType, string? ns)
    {
        var typeName = activityType.Name;
        return ns != null ? $"{ns}.{typeName}" : typeName;
    }
    
    public static string GenerateActivityTypeName(Type activityType)
    {
        var typeName = activityType.Name;
        var ns = GenerateActivityTypeNamespace(activityType);
        return ns != null ? $"{ns}.{typeName}" : typeName;
    }
}