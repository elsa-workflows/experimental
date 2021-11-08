using System;

namespace Elsa.Api.Core.Models
{
    public class ActivityOutputDescriptor : ActivityPropertyDescriptor
    {
        public ActivityOutputDescriptor()
        {
        }

        public ActivityOutputDescriptor(
            string name,
            Type type,
            string? description = default)
        {
            Name = name;
            Type = type;
            Description = description;
        }
    }
}