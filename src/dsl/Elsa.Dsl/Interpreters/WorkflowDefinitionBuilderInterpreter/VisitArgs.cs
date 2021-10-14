using System.Linq;
using Antlr4.Runtime.Tree;
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
                var childContext = (IParseTree)x.expr() ?? x.expr_external();
                return _expressionValue.Get(childContext);
            }).ToList();

            _argValues.Put(context, argValues);

            return DefaultResult;
        }
    }
}