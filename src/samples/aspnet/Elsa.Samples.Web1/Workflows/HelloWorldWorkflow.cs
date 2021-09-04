using Elsa.Activities.Console;
using Elsa.Contracts;

namespace Elsa.Samples.Web1.Workflows
{
    public static class HelloWorldWorkflow
    {
        public static IActivity Create() => new WriteLine("Hello World!");
    }
}