using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public record Workflow(IActivity Root, ICollection<ITrigger>? Triggers = default);
}