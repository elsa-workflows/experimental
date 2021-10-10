using System.Linq;
using Elsa.Contracts;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitArgs(ElsaParser.ArgsContext context)
        {
            var args = context.arg();

            var argValues = args.Select(x =>
            {
                Visit(x);
                return _expressionValue.Get(x.expr());
            }).ToList();

            _argValues.Put(context, argValues);

            return DefaultResult;
        }
    }
}