using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Samples.Console1.Workflows
{
    public static class GreetingWorkflow
    {
        public static IActivity Create()
        {
            var name = new Variable<string>();
            var readLine1 = new ReadLine(name);

            return new Sequence
            {
                Variables = { name },
                Activities =
                {
                    new WriteLine("What's your name?"),
                    readLine1,
                    new WriteLine(context => $"Nice to meet you, {name.Get(context)}!")
                }
            };
        }
    }
}