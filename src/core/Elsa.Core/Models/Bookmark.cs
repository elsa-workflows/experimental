using System;
using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public record Bookmark(INode Target, string Name, IDictionary<string, object?> Data, Action? Resume);
}