using System;
using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Runtime.Models
{
    public record Workflow(string Id, int Version, DateTime CreatedAt, IActivity Root, ICollection<TriggerSource> Triggers);
}