using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.ControlFlow
{
    public class If : Node
    {
        [Input] public IExpression<bool> Condition { get; set; } = new Literal<bool>(false);
        [Port] public INode? True { get; set; }
        [Port] public INode? False { get; set; }
    }

    public class IfDriver : NodeDriver<If>
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        public IfDriver(IExpressionEvaluator expressionEvaluator) => _expressionEvaluator = expressionEvaluator;

        protected override async ValueTask<INodeExecutionResult> ExecuteAsync(If node, NodeExecutionContext context)
        {
            var result = await _expressionEvaluator.EvaluateAsync(node.Condition, context);
            var nextNode = result ? node.True : node.False;
            return nextNode != null ? ScheduleNode(nextNode) : Done();
        }
    }
}