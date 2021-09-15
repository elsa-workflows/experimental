using System;
using Elsa.Attributes;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Console
{
    public class ReadLine : Activity
    {
        public ReadLine()
        {
        }
        
        public ReadLine(Variable variable, Func<object?, object?>? valueConverter = default) => Output = new Output<string?>(variable, valueConverter);

        [Output] public Output<string?>? Output { get; set; }
    }

    public class ReadLineDriver : ActivityDriver<ReadLine>
    {
        protected override void Execute(ReadLine activity, ActivityExecutionContext context)
        {
            var text = System.Console.ReadLine();
            context.Set(activity.Output, text);
        }
    }
}