using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Nodes.ControlFlow
{
    public class If : Node
    {
        [Input] public IExpression<bool> Condition { get; set; } = new Literal<bool>(false);
        [Port] public INode? Then { get; set; }
        [Port] public INode? Else { get; set; }
    }

    public class IfDriver : NodeDriver<If>
    {
        private readonly IExpressionEvaluator _expressionEvaluator;
        public IfDriver(IExpressionEvaluator expressionEvaluator) => _expressionEvaluator = expressionEvaluator;

        protected override async ValueTask ExecuteAsync(If node, NodeExecutionContext context)
        {
            var result = await _expressionEvaluator.EvaluateAsync(node.Condition, context);
            var nextNode = result ? node.Then : node.Else;

            if (nextNode != null)
                context.ScheduleNode(nextNode);
        }
    }
}