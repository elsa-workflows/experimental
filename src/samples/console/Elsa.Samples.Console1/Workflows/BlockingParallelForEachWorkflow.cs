using System.Collections.Generic;
using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Samples.Console1.Workflows
{
    public static class BlockingParallelForEachWorkflow 
    {
        public static IActivity Create()
        {
            var items = new[] { "C#", "Rust", "Go" };
            var currentItem = new Variable<string>();
            
            return new Sequence
            {
                Activities =
                {
                    new WriteLine("Programming languages in parallel:"),
                    new ParallelForEach<string>()
                    {
                        Items = new Input<ICollection<string>>(items),
                        CurrentValue = currentItem,
                        Body = new Sequence
                        {
                            Activities =
                            {
                                new WriteLine(context => currentItem.Get(context)),
                                new Event(context => currentItem.Get(context))
                            }
                        }
                    },
                    new WriteLine("Done.")
                }
            };
        }
    }
}