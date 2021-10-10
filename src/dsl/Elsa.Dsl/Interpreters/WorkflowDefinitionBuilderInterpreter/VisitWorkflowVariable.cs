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
       
        public override IWorkflowDefinitionBuilder VisitVarDecl(ElsaParser.VarDeclContext context)
        {
            var workflowVariableName = context.ID().GetText();

            VisitChildren(context);

            var workflowVariableValue = _expressionValue.Get(context.expr());
            
            var workflowVariable = new Variable
            {
                Name = workflowVariableName
            };
            
            if (workflowVariableValue is IActivity activity)
            {
                // When an activity is assigned to a workflow variable, what we really are doing is setting the variable to the activity's output.
                var activityType = activity.GetType();
                var outputProperty = activityType.GetProperty("Output");

                if (outputProperty == null)
                    throw new Exception("Cannot assign output of an activity that does not have an Output property.");

                var outputValue = Activator.CreateInstance(outputProperty.PropertyType, workflowVariable, default);
                outputProperty.SetValue(activity, outputValue);
            }

            var currentContainer = _containerStack.Peek();
            currentContainer.Variables.Add(workflowVariable);

            return DefaultResult;
        }
    }
}