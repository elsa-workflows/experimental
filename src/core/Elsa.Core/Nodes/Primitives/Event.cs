using System.Collections.Generic;
using Elsa.Attributes;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Nodes.Primitives
{
    public class Event : Node
    {
        public Event(string eventName) => EventName = eventName;

        [Input] public string EventName { get; set; }
        [Output] public object? Payload { get; set; }
    }

    public class EventDriver : NodeDriver<Event>
    {
        protected override void Execute(Event node, NodeExecutionContext context) =>
            context.AddBookmark(nameof(Event), new Dictionary<string, object?> { [nameof(Event.EventName)] = node.EventName });
    }
}