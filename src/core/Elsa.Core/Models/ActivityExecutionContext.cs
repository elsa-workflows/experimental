using System.Collections.Generic;
using System.Threading;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class ActivityExecutionContext
    {
        public ActivityExecutionContext(WorkflowExecutionContext workflowExecutionContext, ScheduledActivity scheduledActivity, ExecuteActivityDelegate? executeDelegate, CancellationToken cancellationToken)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            ScheduledActivity = scheduledActivity;
            ExecuteDelegate = executeDelegate;
            CancellationToken = cancellationToken;
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public ScheduledActivity ScheduledActivity { get; set; }
        public ExecuteActivityDelegate? ExecuteDelegate { get; }
        public CancellationToken CancellationToken { get; }
        public Node Node => WorkflowExecutionContext.FindNodeByActivity(ScheduledActivity.Activity);
        public IActivity Activity => ScheduledActivity.Activity;
        public IEnumerable<Bookmark> Bookmarks => WorkflowExecutionContext.Bookmarks;
        public void ScheduleActivity(IActivity activity) => WorkflowExecutionContext.Schedule(activity);
        public void ScheduleActivities(params IActivity[] activities) => ScheduleActivities((IEnumerable<IActivity>)activities);

        public void ScheduleActivities(IEnumerable<IActivity> activities)
        {
            foreach (var node in activities)
                ScheduleActivity(node);
        }

        public T? GetVariable<T>(string name) => (T?)GetVariable(name);

        public object? GetVariable(string name)
        {
            return default;
        }

        public void SetBookmarks(IEnumerable<Bookmark> bookmarks) => WorkflowExecutionContext.SetBookmarks(bookmarks);
        public void SetBookmark(Bookmark bookmark) => WorkflowExecutionContext.SetBookmark(bookmark);

        public void SetBookmark(string? hash, IDictionary<string, object?>? data = default, ExecuteActivityDelegate? callback = default) => 
            SetBookmark(new Bookmark(ScheduledActivity.Activity.ActivityType, hash, ScheduledActivity.Activity.ActivityId, data ?? new Dictionary<string, object?>(), callback?.Method.Name));

        public bool GetActivityIsCurrentTrigger(IActivity activity) => WorkflowExecutionContext.Trigger?.ActivityId == activity.ActivityId;
    }
}