using System;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

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
        protected override void Execute(For node, NodeExecutionContext context)
        {
            var iterateNode = node.Iterate;

            if (iterateNode == null)
                return;

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
                node.CurrentValue = currentValue;
                context.ScheduleNode(iterateNode);
                return;
            }

            node.CurrentValue = null;

            if (node.Next != null)
                context.ScheduleNode(node.Next);
        }
    }
}