using System;

namespace Elsa.Api.Core.Models;

[Flags]
public enum ActivityTraits
{
    Action = 1,
    Trigger = 2
}