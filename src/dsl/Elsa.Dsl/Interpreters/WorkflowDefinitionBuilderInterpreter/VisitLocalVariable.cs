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
        public override IWorkflowDefinitionBuilder VisitLocalVarDecl(ElsaParser.LocalVarDeclContext context)
        {
            var variableName = context.ID().GetText();
            var variableType = context.type()?.ID().GetText();

            VisitChildren(context);

            var value = _expressionValue.Get(context.expr());

            var variable = new DefinedVariable
            {
                Identifier = variableName,
                Value = value
            };

            _definedVariables[variableName] = variable;

            return DefaultResult;
        }
    }
}