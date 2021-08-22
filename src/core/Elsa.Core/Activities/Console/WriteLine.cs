using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Console
{
    public class WriteLine : CodeActivity
    {
        public WriteLine()
        {
        }
        
        public WriteLine(string text) : this(new Literal<string>(text))
        {
        }

        public WriteLine(Func<string> text) : this(new Delegate<string>(text))
        {
        }
        
        public WriteLine(Func<ActivityExecutionContext, string> text) : this(new Delegate<string>(text))
        {
        }

        public WriteLine(IExpression<string> text) => Text = text;
        public IExpression<string> Text { get; set; } = default!;
    }

    public class WriteLineDriver : ActivityDriver<WriteLine>
    {
        private readonly IExpressionEvaluator _expressionEvaluator;

        public WriteLineDriver(IExpressionEvaluator expressionEvaluator)
        {
            _expressionEvaluator = expressionEvaluator;
        }

        protected override async ValueTask ExecuteAsync(WriteLine activity, ActivityExecutionContext context)
        {
            var text = await _expressionEvaluator.EvaluateAsync(activity.Text, context);
            System.Console.WriteLine(text);
        }
    }
}