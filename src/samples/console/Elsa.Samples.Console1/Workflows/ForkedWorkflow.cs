using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Contracts;

namespace Elsa.Samples.Console1.Workflows
{
    public static class ForkedWorkflow
    {
        public static IActivity Create()
        {
            return new Sequence
            {
                Activities = new IActivity[]
                {
                    new WriteLine("Forking..."),
                    new Fork
                    {
                        Branches = new IActivity[]
                        {
                            new Sequence
                            {
                                Activities = new IActivity[]
                                {
                                    new WriteLine("Branch 1 (blocking)"),
                                    new Event("Branch1"),
                                    new WriteLine("Branch 1 (resumed)"),
                                }
                            },
                            new WriteLine("Branch 2"),
                            new WriteLine("Branch 3")
                        }
                    }
                }
            };
        }
    }
}