using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.Primitives
{
    public class Event : Node, INodeDriver
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

        public ValueTask<INodeExecutionResult> ExecuteAsync(NodeExecutionContext context)
        {
            var node = (Event)context.Node;
            return new ValueTask<INodeExecutionResult>(Bookmark(nameof(Event), new Dictionary<string, object?> { [nameof(Event.EventName)] = node.EventName }));
        }
    }
}