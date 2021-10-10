using Elsa.Contracts;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitIfStat(ElsaParser.IfStatContext context)
        {
            
            
            return DefaultResult;
        }
    }
}