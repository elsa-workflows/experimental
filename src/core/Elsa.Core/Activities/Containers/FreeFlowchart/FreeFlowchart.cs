using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities.Containers.FreeFlowchart
{
    public class FreeFlowchart : Container
    {
        [Outbound]public IActivity Start { get; set; } = default!;
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
        
        protected override void ScheduleChildren(ActivityExecutionContext context)
        {
            if (Start == null!)
                return;
            
            context.ScheduleActivity(Start, OnChildCompleted);
        }

        private ValueTask OnChildCompleted(ActivityExecutionContext context, ActivityExecutionContext childContext)
        {
            ScheduleChildren(childContext, childContext.Activity);
            return ValueTask.CompletedTask;
        }

        private void ScheduleChildren(ActivityExecutionContext context, IActivity parent)
        {
            if (parent == null!)
                return;

            var outboundConnections = Connections.Where(x => x.Source == parent).ToList();
            var children = outboundConnections.Select(x => x.Target).ToList();
            
            context.ScheduleActivities(children, OnChildCompleted);
        }
    }
}