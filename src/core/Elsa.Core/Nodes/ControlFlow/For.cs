using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.ControlFlow
{
    public enum ForOperator
    {
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }

    public class For : Node
    {
        [Input] public int Start { get; set; } = 0;
        [Input] public int End { get; set; }
        [Input] public int Step { get; set; } = 1;
        [Input] public ForOperator Operator { get; set; } = ForOperator.LessThanOrEqual;
        [Port] public INode? Iterate { get; set; }
        [Port] public INode? Next { get; set; }
        public int? CurrentValue { get; set; }
    }

    public class ForDriver : NodeDriver<For>
    {
        protected override ValueTask<INodeExecutionResult> ExecuteAsync(For node, NodeExecutionContext context)
        {
            var iterateNode = node.Iterate;

            if (iterateNode == null)
                return new ValueTask<INodeExecutionResult>(Done());

            var end = node.End;
            var currentValue = node.CurrentValue != null ? node.CurrentValue + node.Step : node.Start;

            var loop = node.Operator switch
            {
                ForOperator.LessThan => currentValue < end,
                ForOperator.LessThanOrEqual => currentValue <= end,
                ForOperator.GreaterThan => currentValue > end,
                ForOperator.GreaterThanOrEqual => currentValue >= end,
                _ => throw new NotSupportedException()
            };

            if (loop)
            {
                var variables = new Dictionary<string, object?> { ["CurrentValue"] = currentValue };
                node.CurrentValue = currentValue;
                return new ValueTask<INodeExecutionResult>(ScheduleNode(iterateNode, node, variables));
            }

            node.CurrentValue = null;
            var result = node.Next != null ? (INodeExecutionResult)ScheduleNode(node.Next) : Done();
            return new ValueTask<INodeExecutionResult>(result);
        }
    }
}