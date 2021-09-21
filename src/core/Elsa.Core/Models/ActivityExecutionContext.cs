using System;
using System.Collections.Generic;
using System.Threading;
using Elsa.Contracts;

namespace Elsa.Models
{
    public class ActivityExecutionContext
    {
        public ActivityExecutionContext(
            WorkflowExecutionContext workflowExecutionContext,
            ActivityExecutionContext? parentActivityExecutionContext,
            ScheduledActivity scheduledActivity,
            ExecuteActivityDelegate? executeDelegate,
            CancellationToken cancellationToken)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            ParentActivityExecutionContext = parentActivityExecutionContext;
            ScheduledActivity = scheduledActivity;
            ExecuteDelegate = executeDelegate;
            CancellationToken = cancellationToken;
            Register = workflowExecutionContext.GetOrCreateRegister(scheduledActivity.Activity);
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public ActivityExecutionContext? ParentActivityExecutionContext { get; set; }
        public ScheduledActivity ScheduledActivity { get; set; }
        public ExecuteActivityDelegate? ExecuteDelegate { get; }
        public CancellationToken CancellationToken { get; }
        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();
        public Node Node => WorkflowExecutionContext.FindNodeByActivity(ScheduledActivity.Activity);
        public IActivity Activity => ScheduledActivity.Activity;
        public IEnumerable<Bookmark> Bookmarks => WorkflowExecutionContext.Bookmarks;
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

        public void SetBookmarks(IEnumerable<Bookmark> bookmarks) => WorkflowExecutionContext.SetBookmarks(bookmarks);
        public void SetBookmark(Bookmark bookmark) => WorkflowExecutionContext.SetBookmark(bookmark);

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

        public Register Register { get; }

        public RegisterLocation GetLocation(RegisterLocationReference locationReference) => locationReference.GetLocation(Register);
        public object Get(RegisterLocationReference locationReference) => GetLocation(locationReference).Value!;
        public T Get<T>(RegisterLocationReference locationReference) => (T)Get(locationReference);
        public T? Get<T>(Input<T> input) => (T?)input.LocationReference.GetLocation(Register).Value;

        public void Set(RegisterLocationReference locationReference, object? value)
        {
            var location = locationReference.GetLocation(Register);
            location.Value = value;
        }

        public void Set(Output? output, object? value)
        {
            if (output?.LocationReference == null)
                return;

            var convertedValue = output.ValueConverter?.Invoke(value) ?? value;
            Set(output.LocationReference, convertedValue);
        }
        
        public T GetRequiredService<T>() where T : notnull => WorkflowExecutionContext.GetRequiredService<T>();

        public void Cleanup()
        {
            // Delete register.
            WorkflowExecutionContext.RemoveRegister(Activity);
            
            // Pop out of workflow context.
            WorkflowExecutionContext.ActivityExecutionContexts.Pop();
        }
    }
}