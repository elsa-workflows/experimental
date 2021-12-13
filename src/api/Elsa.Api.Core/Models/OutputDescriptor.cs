using System;

namespace Elsa.Api.Core.Models;

public class OutputDescriptor : PropertyDescriptor
{
    public OutputDescriptor()
    {
    }

    public OutputDescriptor(
        string name,
        Type type,
        string? description = default)
    {
        Name = name;
        Type = type;
        Description = description;
    }
}