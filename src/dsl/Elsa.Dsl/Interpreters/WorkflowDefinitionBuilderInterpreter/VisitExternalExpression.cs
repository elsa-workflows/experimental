using Elsa.Contracts;
using Elsa.Expressions;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitExpr_external(ElsaParser.Expr_externalContext context)
        {
            var language = context.ID();
            var expression = context.expr_external_value().GetText();

            // TODO: Construct an `Input<>` with the appropriate expression object based on the specified language.
            _expressionValue.Put(context, expression);
            
            return DefaultResult;
        }
    }
}