using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Nodes.Console;
using Elsa.Nodes.Containers;
using Elsa.Nodes.ControlFlow;

namespace Elsa.Samples.Console1.Workflows
{
    public static class ConditionalWorkflow
    {
        public static IActivity Create()
        {
            var readLine1 = new ReadLine();

            return new Sequence
            {
                Activities = new IActivity[]
                {
                    new WriteLine("What's your age?"),
                    readLine1,
                    new If
                    {
                        Condition = new Delegate<bool>(() => int.Parse(readLine1.Output!) >= 16),
                        Then = new Sequence
                        {
                            Activities = new IActivity[]
                            {
                                new WriteLine("Enjoy your driver's license!"),
                                new WriteLine("But be careful!")
                            }
                        },
                        Else = new WriteLine("Enjoy your bicycle!")
                    }
                }
            };
        }
    }
}