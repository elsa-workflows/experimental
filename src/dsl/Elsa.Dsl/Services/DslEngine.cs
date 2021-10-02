using System;
using System.Collections.Generic;
using System.Threading;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Elsa.Activities.Console;
using Elsa.Contracts;
using Elsa.Dsl.Contracts;
using Elsa.Dsl.Interpreters;
using Elsa.Models;

namespace Elsa.Dsl.Services
{
    public class DslEngine : IDslEngine
    {
        private readonly ITriggerTypeRegistry _triggerTypeRegistry;

        public DslEngine(ITriggerTypeRegistry triggerTypeRegistry)
        {
            _triggerTypeRegistry = triggerTypeRegistry;
        }
        
        public Workflow Parse(string script)
        {
            var stream = CharStreams.fromString(script);
            var lexer = new ElsaLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ElsaParser(tokens);
            var tree = parser.file();

            var interpreter = new WorkflowModelInterpreter(_triggerTypeRegistry);
            var result = interpreter.Visit(tree);
            var triggers = new List<ITrigger>();
            var root = new WriteLine();
            var workflow = new Workflow("id", 1, DateTime.Now, root, triggers);

            return workflow;
        }
    }
}