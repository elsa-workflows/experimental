using System;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Activities.Console
{
    public class WriteLine : Activity
    {
        public WriteLine()
        {
        }
        
        public WriteLine(string text) : this(new Literal<string>(text))
        {
        }
        
        public WriteLine(Func<string> text) : this(new DelegateReference<string>(text))
        {
        }
        
        public WriteLine(Func<ActivityExecutionContext, string> text) : this(new DelegateReference<string>(text))
        {
        }

        public WriteLine(Variable<string> variable) => Text = new Input<string>(variable);
        public WriteLine(Literal<string> literal) => Text = new Input<string>(literal);
        public WriteLine(DelegateReference delegateExpression) => Text = new Input<string>(delegateExpression);
        public WriteLine(Input<string> text) => Text = text;
        public Input<string> Text { get; set; } = default!;
    }

    public class WriteLineDriver : ActivityDriver<WriteLine>
    {
        protected override void Execute(WriteLine activity, ActivityExecutionContext context)
        {
            var text = context.Get(activity.Text);
            System.Console.WriteLine(text);
        }
    }
}