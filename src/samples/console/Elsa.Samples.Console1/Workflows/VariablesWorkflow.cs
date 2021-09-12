using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Samples.Console1.Workflows
{
    public static class VariablesWorkflow
    {
        public static IActivity Create()
        {
            var message = new Variable<string>("Hello World!");
            
            return new Sequence
            {
                Variables = { message },
                Activities =
                {
                    new WriteLine(message)
                }
            };
        }
    }
}