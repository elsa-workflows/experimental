using System.Linq;
using Elsa.Contracts;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitFuncCall(ElsaParser.FuncCallContext context)
        {
            VisitChildren(context);
            
            var functionName = context.ID().GetText();
            var args = _argValues.Get(context.args());
            
            
            return DefaultResult;
        }
    }
}