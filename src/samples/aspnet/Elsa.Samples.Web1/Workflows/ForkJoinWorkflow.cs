using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Samples.Web1.Workflows
{
    public class ForkJoinWorkflow : IWorkflow
    {
        public string Id => nameof(ForkJoinWorkflow);
        public int Version => 1;

        public void Build(IWorkflowBuilder builder)
        {
            var join = new Join();
            
            builder.Root = new Sequence
            {
                Activities =
                {
                    new Fork
                    {
                        JoinMode = new Input<JoinMode>(JoinMode.WaitAny),
                        Branches =
                        {
                            new Sequence
                            {
                                Activities =
                                {
                                    new Event("branch1"),
                                    new WriteLine("Branch 1 continues!"),
                                    join
                                }
                            },
                            new Sequence
                            {
                                Activities =
                                {
                                    new Event("branch2"),
                                    new WriteLine("Branch 2 continues!"),
                                    join
                                }
                            }
                        }
                    }
                }
            };

            join.Next = new Sequence
            {
                Activities =
                {
                    new WriteLine("Done!")
                } 
            };
        }
    }
}