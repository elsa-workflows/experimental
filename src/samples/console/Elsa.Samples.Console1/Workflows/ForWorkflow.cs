using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Contracts;

namespace Elsa.Samples.Console1.Workflows
{
    public static class ForWorkflow
    {
        public static IActivity Create()
        {
            var for1 = new For
            {
                Start = 1,
                End = 3,
                Next = new WriteLine("Done.")
            };

            for1.Iterate = new WriteLine(context => $"Current value: {for1.CurrentValue.Get<int>(context)}");

            return new Sequence
            (
                new WriteLine(() => $"Counting numbers from {for1.Start} to {for1.End}:"),
                for1,
                new WriteLine("End of workflow")
            );
        }
    }
}