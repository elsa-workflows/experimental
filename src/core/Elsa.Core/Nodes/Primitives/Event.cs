using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.Primitives
{
    public class Event : Node
    {
        public Event()
        {
        }

        public Event(string eventName)
        {
            EventName = eventName;
        }

        [Input] public string EventName { get; set; } = default!;
        [Output] public object? Payload { get; set; }

        public bool GetSupportsNode(INode node) => node is Event;
        public int Priority => 0;
    }

    public class EventDriver : NodeDriver<Event>
    {
        protected override INodeExecutionResult Execute(Event node, NodeExecutionContext context)
        {
            return Bookmark(nameof(Event), new Dictionary<string, object?> { [nameof(Event.EventName)] = node.EventName }, ResumeAsync);
        }

        public ValueTask<INodeExecutionResult> ResumeAsync(NodeExecutionContext context)
        {
            System.Console.WriteLine("Resumed!");
            return new ValueTask<INodeExecutionResult>(Done());
        }
    }
}