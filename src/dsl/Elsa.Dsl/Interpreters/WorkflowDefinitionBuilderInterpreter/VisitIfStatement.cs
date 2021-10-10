using Elsa.Activities.ControlFlow;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitIfStat(ElsaParser.IfStatContext context)
        {
            var ifActivity = new If();
            var conditionExpr = context.expr().GetText();

            ifActivity.Condition = new Input<bool>(new ElsaExpression(conditionExpr));
            
            var thenStat = context.thenStat().stat();
            var elseStat = context.elseStat()?.stat();
            
            Visit(thenStat);

            var thenActivity = _expressionValue.Get(thenStat);
            ifActivity.Then = (IActivity?)thenActivity;

            if (elseStat != null)
            {
                Visit(elseStat);
                var elseActivity = _expressionValue.Get(elseStat);
                ifActivity.Else = (IActivity?)elseActivity;
            }
            
            _expressionValue.Put(context, ifActivity);
            return DefaultResult;
        }
    }
}