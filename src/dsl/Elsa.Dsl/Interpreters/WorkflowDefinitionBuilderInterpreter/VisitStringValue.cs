using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Antlr4.Runtime.Tree;
using Elsa.Builders;
using Elsa.Contracts;
using Elsa.Dsl.Extensions;
using Elsa.Dsl.Models;
using Elsa.Models;

namespace Elsa.Dsl.Interpreters
{
    public partial class WorkflowDefinitionBuilderInterpreter
    {
        public override IWorkflowDefinitionBuilder VisitStringValueExpr(ElsaParser.StringValueExprContext context)
        {
            var value = context.GetText().Trim('\"');
            _expressionValue.Put(context, value);
            return DefaultResult;
        }
    }
}