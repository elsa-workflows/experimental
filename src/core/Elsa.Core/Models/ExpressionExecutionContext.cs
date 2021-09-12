namespace Elsa.Models
{
    public class ExpressionExecutionContext
    {
        public ActivityExecutionContext ActivityExecutionContext { get; }

        public ExpressionExecutionContext(ActivityExecutionContext activityExecutionContext)
        {
            ActivityExecutionContext = activityExecutionContext;
        }
    }
}