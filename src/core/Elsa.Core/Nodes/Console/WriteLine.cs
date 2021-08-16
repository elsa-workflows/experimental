using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.Console
{
    public class WriteLine : INode
    {
        public WriteLine(string text) : this(new Literal<string>(text))
        {
        }

        public WriteLine(Func<string> text) : this(new Delegate<string>(text))
        {
        }
        
        public WriteLine(Func<NodeExecutionContext, string> text) : this(new Delegate<string>(text))
        {
        }

        public WriteLine(IExpression<string> text) => Text = text;
        public IExpression<string> Text { get; set; }
    }

    public class WriteLineDriver : NodeDriver<WriteLine>
    {
        private readonly IExpressionEvaluator _expressionEvaluator;

        public WriteLineDriver(IExpressionEvaluator expressionEvaluator)
        {
            _expressionEvaluator = expressionEvaluator;
        }

        protected override async ValueTask<INodeExecutionResult> ExecuteAsync(WriteLine node, NodeExecutionContext context)
        {
            var text = await _expressionEvaluator.EvaluateAsync(node.Text, context);
            System.Console.WriteLine(text);
            return Done();
        }
    }
}