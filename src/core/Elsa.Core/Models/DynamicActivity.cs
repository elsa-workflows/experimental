using System.Collections.Generic;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class DynamicActivity : IActivity
    {
        public DynamicActivity(string activityType, IDictionary<string, object?>? input = default)
        {
            ActivityType = activityType;

            if (input != null)
                Input = input;
        }

        public string ActivityId { get; set; } = default!;
        public string ActivityType { get; set; }
        public IDictionary<string, object?> Input { get; set; } = new Dictionary<string, object?>();
        public IDictionary<string, object?> Output { get; set; } = new Dictionary<string, object?>();
        public IDictionary<string, IActivity?> Ports { get; set; } = new Dictionary<string, IActivity?>();
    }
}