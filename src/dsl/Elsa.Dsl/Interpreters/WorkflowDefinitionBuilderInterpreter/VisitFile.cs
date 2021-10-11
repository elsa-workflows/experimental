using System.Linq;
using Elsa.Activities.Containers;
using Elsa.Contracts;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitFile(ElsaParser.FileContext context)
        {
            VisitChildren(context);

            var objectStats = context.stat().OfType<ElsaParser.ObjectStatContext>().ToList();
            var blockStats = context.stat().OfType<ElsaParser.BlockStatContext>().ToList();
            var activities = objectStats.Select(x => (IActivity)_object.Get(x)!).ToList();
            activities.AddRange(blockStats.Select(x => (Sequence)_expressionValue.Get(x)!));
            
            var rootActivity = activities.Count == 1 ? activities.Single() : new Sequence(activities.ToArray());

            _workflowDefinitionBuilder.WithRoot(rootActivity);
            
            return DefaultResult;
        }
    }
}