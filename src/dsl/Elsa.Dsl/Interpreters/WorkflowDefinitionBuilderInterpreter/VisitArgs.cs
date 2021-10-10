using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Dsl.Models;

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