using System;
using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Samples.Console1.Workflows
{
    public static class DynamicActivityWorkflow
    {
        public static IActivity Create() => new DynamicActivity("MyWriteLine", new Dictionary<string, object?> { ["Text"] = new LiteralExpression<string>("Hello World!") });
    }
    
    public class MyWriteLineDriver : DynamicActivityDriver
    {
        protected override string ActivityType => "MyWriteLine";

        protected override void Execute(DynamicActivity dynamicActivity, ActivityExecutionContext context)
        {
            var text = dynamicActivity.Input["Text"]!;
            Console.WriteLine(text);
        }
    }
}