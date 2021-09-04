using Elsa.Activities.Console;
using Elsa.Runtime.Contracts;

namespace Elsa.Samples.Web1.Workflows
{
    public class HelloWorldWorkflow : IWorkflow
    {
        public string Id => nameof(HelloWorldWorkflow);
        public int Version => 1;
        public void Build(IWorkflowBuilder builder)
        {
            builder.Root = new WriteLine("Hello World!");
        }
    }
}