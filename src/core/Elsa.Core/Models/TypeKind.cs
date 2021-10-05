using System;

namespace Elsa.Models
{
    [Flags]
    public enum TypeKind
    {
        Unknown = 0,
        Activity = 1,
        Trigger = 2
    }
}