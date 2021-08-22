using Elsa.Contracts;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;

namespace Elsa.Samples.Console1.Workflows
{
    public static class HelloGoodbyeWorkflow
    {
        public static INode Create() =>
            new Sequence
            {
                Nodes = new INode[]
                {
                    new WriteLine("Hello World!"),
                    new WriteLine("Goodbye cruel world...")
                }
            };
    }
}