using Elsa.Activities.Console;
using Elsa.Activities.Containers.FreeFlowchart;
using Elsa.Activities.ControlFlow;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Samples.Console1.Workflows
{
    public static class FreeFlowchartWorkflow
    {
        public static IActivity Create()
        {
            var age = new Variable<int>();
            var flowchart = new FreeFlowchart();
            var step1 = new WriteLine("Please enter your age.");
            var step2 = new ReadLine(age, x => int.Parse((string)x!));
            var step3 = new If
            {
                Condition = new Input<bool>(context => age.Get(context) > 16),
                Then = new WriteLine("Enjoy your beer!"),
                Else = new WriteLine("Enjoy your soda!")
            };
            var step4 = new WriteLine("Done");
            var connection1 = new Connection(step1, step2);
            var connection2 = new Connection(step2, step3);
            var connection3 = new Connection(step3, step4);
            flowchart.Start = step1;
            flowchart.Activities.Add(step2);
            flowchart.Activities.Add(step3);
            flowchart.Activities.Add(step4);
            flowchart.Connections.Add(connection1);
            flowchart.Connections.Add(connection2);
            flowchart.Connections.Add(connection3);
            flowchart.Variables.Add(age);

            return flowchart;
        }
    }
}