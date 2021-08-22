using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.ControlFlow
{
    public class If : CodeActivity
    {
        [Input] public IExpression<bool> Condition { get; set; } = new Literal<bool>(false);
        [Port] public IActivity? Then { get; set; }
        [Port] public IActivity? Else { get; set; }
    }

    public class IfDriver : ActivityDriver<If>
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        public IfDriver(IExpressionEvaluator expressionEvaluator) => _expressionEvaluator = expressionEvaluator;

        protected override async ValueTask ExecuteAsync(If activity, ActivityExecutionContext context)
        {
            var result = await _expressionEvaluator.EvaluateAsync(activity.Condition, context);
            var nextNode = result ? activity.Then : activity.Else;

            if (nextNode != null)
                context.ScheduleActivity(nextNode);
        }
    }
}