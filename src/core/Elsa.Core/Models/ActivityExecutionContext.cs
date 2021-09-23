using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class ActivityExecutionContext
    {
        private readonly List<Bookmark> _bookmarks = new();
        
        public ActivityExecutionContext(
            WorkflowExecutionContext workflowExecutionContext,
            ExpressionExecutionContext expressionExecutionContext,
            //ActivityExecutionContext? parentActivityExecutionContext,
            ScheduledActivity scheduledActivity,
            CancellationToken cancellationToken)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            ExpressionExecutionContext = expressionExecutionContext;
            //ParentActivityExecutionContext = parentActivityExecutionContext;
            ScheduledActivity = scheduledActivity;
            CancellationToken = cancellationToken;
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public ExpressionExecutionContext ExpressionExecutionContext { get; }
        //public ActivityExecutionContext? ParentActivityExecutionContext { get; set; }
        public ScheduledActivity ScheduledActivity { get; set; }
        public ExecuteActivityDelegate? ExecuteDelegate { get; set; }
        public CancellationToken CancellationToken { get; }
        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();
        public Node Node => WorkflowExecutionContext.FindNodeByActivity(ScheduledActivity.Activity);
        public IActivity Activity => ScheduledActivity.Activity;
        public IReadOnlyCollection<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);
        public Register Register => ExpressionExecutionContext.Register;

        public void ScheduleActivity(IActivity activity, ActivityCompletionCallback? completionCallback = default) => WorkflowExecutionContext.Schedule(activity, Activity, completionCallback);
        public void ScheduleActivity(IActivity activity, IActivity owner, ActivityCompletionCallback? completionCallback = default) => WorkflowExecutionContext.Schedule(activity, owner, completionCallback);
        public void ScheduleActivities(params IActivity[] activities) => ScheduleActivities((IEnumerable<IActivity>)activities);

        public void ScheduleActivities(IEnumerable<IActivity> activities, ActivityCompletionCallback? completionCallback = default)
        {
            foreach (var activity in activities)
                ScheduleActivity(activity, completionCallback);
        }

        public T? GetVariable<T>(string name) => (T?)GetVariable(name);

        public object? GetVariable(string name)
        {
            return default;
        }

        public void SetBookmarks(IEnumerable<Bookmark> bookmarks) => _bookmarks.AddRange(bookmarks);
        public void SetBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);

        public void SetBookmark(string? hash, IDictionary<string, object?>? data = default, ExecuteActivityDelegate? callback = default) =>
            SetBookmark(new Bookmark(
                Guid.NewGuid().ToString(),
                ScheduledActivity.Activity.ActivityType,
                hash,
                ScheduledActivity.Activity.ActivityId,
                data ?? new Dictionary<string, object?>(),
                callback?.Method.Name));

        public bool GetActivityIsCurrentTrigger(IActivity activity) => WorkflowExecutionContext.Trigger?.ActivityId == activity.ActivityId;
        public T? GetProperty<T>(string key) => Properties.TryGetValue(key, out var value) ? (T?)value : default(T);
        public void SetProperty<T>(string key, T value) => Properties[key] = value;

        public T GetRequiredService<T>() where T : notnull => WorkflowExecutionContext.GetRequiredService<T>();

        public void Cleanup()
        {
            // Delete register.
            WorkflowExecutionContext.RemoveRegister(Activity);
            
            // Pop out of workflow context.
            WorkflowExecutionContext.ActivityExecutionContexts.Remove(this);
        }

        public T? Get<T>(Input<T> input) => ExpressionExecutionContext.Get(input);
        public object Get(RegisterLocationReference locationReference) => ExpressionExecutionContext.GetLocation(locationReference);
        public T Get<T>(RegisterLocationReference locationReference) => ExpressionExecutionContext.Get<T>(locationReference);
        public void Set(RegisterLocationReference locationReference, object? value) => ExpressionExecutionContext.Set(locationReference, value);
        public void Set(Output? output, object? value) => ExpressionExecutionContext.Set(output, value);
    }
}