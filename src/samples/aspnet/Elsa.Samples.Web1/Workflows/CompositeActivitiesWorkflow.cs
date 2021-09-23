using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Models;
using Elsa.Runtime.Contracts;
using Elsa.Samples.Web1.Activities;

namespace Elsa.Samples.Web1.Workflows
{
    public class CustomActivitiesWorkflow : IWorkflow
    {
        public string Id => nameof(CustomActivitiesWorkflow);
        public int Version => 1;

        public void Build(IWorkflowBuilder builder)
        {
            var name = new Variable<string>();

            builder.Root = new Sequence
            {
                Variables = { name },
                Activities =
                {
                    new MyGreeterComposite
                    {
                        Name = new Output<string?>(name)
                    },
                    new WriteLine(context => $"Captured name: {name.Get(context)}")
                }
            };
        }
    }
}