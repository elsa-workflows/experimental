using Elsa.Contracts;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Elsa.Nodes.ControlFlow;

namespace Elsa.Samples.Console1.Workflows
{
    public static class ForEachWorkflow
    {
        public static IActivity Create()
        {
            var for1 = new For
            {
                Start = 1,
                End = 3,
                Next = new WriteLine("Done.")
            };

            for1.Iterate = new WriteLine(() => for1.CurrentValue.ToString()!);

            return new Sequence
            {
                Activities = new IActivity[]
                {
                    new WriteLine(() => $"Counting numbers from {for1.Start} to {for1.End}:"),
                    for1
                }
            };
        }
    }
}