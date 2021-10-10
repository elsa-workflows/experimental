using System.Linq;
using Elsa.Activities.Containers;
using Elsa.Contracts;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitFile(ElsaParser.FileContext context)
        {
            var otherStats = context.stat().Where(x => x is not ElsaParser.ObjectStatContext).ToList();
            
            //var rootActivities = objectStats.Select(x => (IActivity)_object.Get(x.@object())).ToList();
            //var rootActivity = rootActivities.Count == 1 ? rootActivities.Single() : new Sequence(rootActivities.ToArray());

            //_workflowDefinitionBuilder.WithRoot(rootActivity);

            // if (rootActivity is IContainer container)
            //     _containerStack.Push(container);

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