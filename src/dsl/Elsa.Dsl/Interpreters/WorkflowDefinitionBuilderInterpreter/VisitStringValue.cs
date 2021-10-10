﻿using Elsa.Contracts;

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