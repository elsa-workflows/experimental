using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Samples.Console1.Workflows
{
    public static class DynamicActivityWorkflow
    {
        public static IActivity Create() => new Activity("MyWriteLine", new Dictionary<string, object?> { ["Text"] = new Literal<string>("Hello World!") });
    }
    
    public class MyWriteLineDriver : DynamicActivityDriver
    {
        private readonly IExpressionEvaluator _expressionEvaluator;

        public MyWriteLineDriver(IExpressionEvaluator expressionEvaluator)
        {
            _expressionEvaluator = expressionEvaluator;
        }
        
        protected override string ActivityType => "MyWriteLine";

        protected override async ValueTask ExecuteAsync(Activity activity, ActivityExecutionContext context)
        {
            var textExpression = (IExpression<string>)activity.Input["Text"]!;
            var text = await _expressionEvaluator.EvaluateAsync(textExpression, context);
            Console.WriteLine(text);
        }
    }
}