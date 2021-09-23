using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Primitives
{
    public class Event : Activity
    {
        public Event()
        {
        }

        public Event(string eventName) => EventName = eventName;

        [Input] public string EventName { get; set; } = default!;
        [Output] public object? Payload { get; set; }
        
        protected override void Execute(ActivityExecutionContext context)
        {
            var hasher = context.GetRequiredService<IHasher>();
            var hash = hasher.Hash(EventName);
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