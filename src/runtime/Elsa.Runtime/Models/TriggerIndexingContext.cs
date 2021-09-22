using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Runtime.Models
{
    public class TriggerIndexingContext
    {
        public WorkflowIndexingContext WorkflowIndexingContext { get; }
        public ExpressionExecutionContext ExpressionExecutionContext { get; }
        public IActivity Activity { get; }

        public TriggerIndexingContext(WorkflowIndexingContext workflowIndexingContext, ExpressionExecutionContext expressionExecutionContext, IActivity activity)
        {
            WorkflowIndexingContext = workflowIndexingContext;
            ExpressionExecutionContext = expressionExecutionContext;
            Activity = activity;
        }
    }
}