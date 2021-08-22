using System.Collections.Generic;
using Elsa.Attributes;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Primitives
{
    public class Event : CodeActivity
    {
        public Event()
        {
        }
        
        public Event(string eventName) => EventName = eventName;

        [Input] public string EventName { get; set; } = default!;
        [Output] public object? Payload { get; set; }
    }

    public class EventDriver : ActivityDriver<Event>
    {
        protected override void Execute(Event activity, ActivityExecutionContext context) =>
            context.SetBookmark(nameof(Event), new Dictionary<string, object?> { [nameof(Event.EventName)] = activity.EventName });
    }
}