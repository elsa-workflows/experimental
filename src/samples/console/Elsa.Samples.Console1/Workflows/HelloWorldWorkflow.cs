using Elsa.Contracts;
using Elsa.Nodes.Console;

namespace Elsa.Samples.Console1.Workflows
{
    public static class HelloWorldWorkflow
    {
        public static INode Create() => new WriteLine("Hello World!");
    }
}