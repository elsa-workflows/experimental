using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Samples.Console1.Workflows
{
    public static class VariablesWorkflow
    {
        public static IActivity Create()
        {
            var message = new Variable<string>("Hello World!");
            var name = new Variable<string>();
            
            return new Sequence
            {
                Variables = { message, name },
                Activities =
                {
                    new WriteLine
                    {
                        Text = new Input<string>(new DelegateReference(context => $"Message: {message.Get(context)}"))
                    },
                    new WriteLine("What is your name?"),
                    new ReadLine
                    {
                        Output = new Output<string?>(name) 
                    },
                    new WriteLine(new DelegateReference(context => $"Nice to meet you, {name.Get(context)}!")),
                }
            };
        }
    }
}