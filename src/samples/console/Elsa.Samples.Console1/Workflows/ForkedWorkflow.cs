using Elsa.Contracts;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Elsa.Nodes.ControlFlow;
using Elsa.Nodes.Primitives;

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