using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Contracts;

namespace Elsa.Samples.Console1.Workflows
{
    public static class GreetingWorkflow
    {
        public static IActivity Create()
        {
            var readLine1 = new ReadLine();

            return new Sequence(
                new WriteLine("What's your name?"),
                readLine1,
                new WriteLine(() => $"Nice to meet you, {readLine1.Output}!")
            );
        }
    }
}