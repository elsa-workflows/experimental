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
        public override IWorkflowDefinitionBuilder VisitIfStat(ElsaParser.IfStatContext context)
        {
            
            
            return DefaultResult;
        }
    }
}