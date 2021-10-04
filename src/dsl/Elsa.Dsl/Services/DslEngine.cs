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
        private readonly IActivityTypeRegistry _activityTypeRegistry;

        public DslEngine(ITriggerTypeRegistry triggerTypeRegistry, IActivityTypeRegistry activityTypeRegistry)
        {
            _triggerTypeRegistry = triggerTypeRegistry;
            _activityTypeRegistry = activityTypeRegistry;
        }
        
        public WorkflowDefinition Parse(string script)
        {
            var stream = CharStreams.fromString(script);
            var lexer = new ElsaLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new ElsaParser(tokens);
            var tree = parser.file();

            var interpreter = new WorkflowDefinitionBuilderInterpreter(_triggerTypeRegistry, _activityTypeRegistry);
            var workflowBuilder = interpreter.Visit(tree);
            var workflow = workflowBuilder.BuildWorkflow();

            return workflow;
        }
    }
}