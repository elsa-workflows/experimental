using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
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
        private readonly IHasher _hasher;

        public EventDriver(IHasher hasher)
        {
            _hasher = hasher;
        }
        
        protected override void Execute(Event activity, ActivityExecutionContext context)
        {
            var hash = _hasher.Hash(activity.EventName);
            context.SetBookmark(hash, callback: Resume);
        }

        private ValueTask Resume(ActivityExecutionContext context)
        {
            var eventActivity = (Event)context.Activity;
            //eventActivity.Payload = context.wo
            
            return ValueTask.CompletedTask;
        }
    }
}