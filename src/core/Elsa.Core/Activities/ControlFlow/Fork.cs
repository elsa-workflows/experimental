using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Extensions;
using Elsa.Models;

namespace Elsa.Activities.ControlFlow
{
    public enum JoinMode
    {
        WaitAny,
        WaitAll
    }
    
    public class Fork : Activity, IContainer
    {
        [Input] public Input<JoinMode> JoinMode { get; set; } = new(ControlFlow.JoinMode.WaitAny);
        [Outbound] public ICollection<IActivity> Branches { get; set; } = new List<IActivity>();
        [Outbound] public IActivity? Next { get; set; }

        protected override void Execute(ActivityExecutionContext context) => context.ScheduleActivities(Branches.Reverse());

        public ValueTask CompleteChildAsync(ActivityExecutionContext context, ActivityExecutionContext childContext)
        {
            OnChildCompleted(context, childContext);
            return ValueTask.CompletedTask;
        }

        private void OnChildCompleted(ActivityExecutionContext context, ActivityExecutionContext childContext)
        {
            if (Next == null)
                return;
            
            var completedChildActivityId = childContext.Activity.ActivityId;
            
            // Ignore Next activity.
            if(completedChildActivityId == Next.ActivityId)
                return;

            // Append activity to set of completed activities.
            var completedActivityIds = context.UpdateProperty<HashSet<string>>("Completed", set =>
            {
                set ??= new HashSet<string>();
                set.Add(completedChildActivityId);
                return set;
            });

            var allChildActivityIds = Branches.Select(x => x.ActivityId).ToImmutableHashSet();
            var joinMode = context.Get(JoinMode);

            switch (joinMode)
            {
                case ControlFlow.JoinMode.WaitAny:
                {
                    // Remove any and all bookmarks from other branches.
                    RemoveBookmarks(context);
                    context.ScheduleActivity(Next);
                }
                    break;
                case ControlFlow.JoinMode.WaitAll:
                {
                    var allSet = allChildActivityIds.All(x => completedActivityIds.Contains(x));

                    if (allSet)
                        context.ScheduleActivity(Next);
                }
                    break;
            }
        }

        private void RemoveBookmarks(ActivityExecutionContext context)
        {
            // Find all descendants for each branch.
            var workflowExecutionContext = context.WorkflowExecutionContext;
            var forkNode = context.ActivityNode;
            var nextNode = forkNode.Children.FirstOrDefault(x => x.Activity == Next);
            var branchNodes = forkNode.Children.Where(x => x != nextNode).ToList();
            var branchDescendantActivityIds = branchNodes.SelectMany(x => x.Flatten()).Select(x => x.Activity.ActivityId).ToHashSet();
            var bookmarksToRemove = context.WorkflowExecutionContext.Bookmarks.Where(x => branchDescendantActivityIds.Contains(x.ActivityId)).ToList();

            workflowExecutionContext.UnregisterBookmarks(bookmarksToRemove);
        }
    }
}