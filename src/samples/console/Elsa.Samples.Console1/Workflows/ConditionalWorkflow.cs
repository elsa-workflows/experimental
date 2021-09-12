using Elsa.Activities.Console;
using Elsa.Activities.Containers;
using Elsa.Activities.ControlFlow;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;

namespace Elsa.Samples.Console1.Workflows
{
    public static class ConditionalWorkflow
    {
        public static IActivity Create()
        {
            var readLine1 = new ReadLine();

            return new Sequence(
                new WriteLine("What's your age?"),
                readLine1,
                new If
                {
                    Condition = new Input<bool>(() => int.Parse(readLine1.Output!) >= 16),
                    Then = new Sequence(
                        new WriteLine("Enjoy your driver's license!"),
                        new WriteLine("But be careful!")),
                    Else = new WriteLine("Enjoy your bicycle!")
                });
        }
    }
}