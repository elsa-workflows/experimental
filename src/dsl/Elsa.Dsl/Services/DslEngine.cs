using Antlr4.Runtime;
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
        
        public WorkflowDefinition Parse(string script)
        {
            var stream = CharStreams.fromString(script);
            var lexer = new ElsaLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ElsaParser(tokens);
            var tree = parser.file();

            var interpreter = new WorkflowDefinitionBuilderInterpreter(_triggerTypeRegistry);
            var workflowBuilder = interpreter.Visit(tree);
            var workflow = workflowBuilder.BuildWorkflow();

            return workflow;
        }
    }
}