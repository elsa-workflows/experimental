using System.Collections.Generic;

namespace Elsa.Api.Core.Models
{
    public class ActivityDescriptor
    {
        public string ActivityType { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public ICollection<Port> InboundPorts { get; set; } = new List<Port>();
        public ICollection<Port> OutboundPorts { get; set; } = new List<Port>();
        public ICollection<ActivityInputDescriptor> InputProperties { get; set; } = new List<ActivityInputDescriptor>();
        public ICollection<ActivityOutputDescriptor> OutputProperties { get; set; } = new List<ActivityOutputDescriptor>();
    }
}