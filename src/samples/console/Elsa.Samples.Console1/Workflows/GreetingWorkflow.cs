using Elsa.Contracts;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;

namespace Elsa.Samples.Console1.Workflows
{
    public static class GreetingWorkflow
    {
        public static IActivity Create()
        {
            var readLine1 = new ReadLine();

            return new Sequence
            {
                Activities = new IActivity[]
                {
                    new WriteLine("What's your name?"),
                    readLine1,
                    new WriteLine(() => $"Nice to meet you, {readLine1.Output}!")
                }
            };
        }
    }
}