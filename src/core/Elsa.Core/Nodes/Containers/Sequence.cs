using System.Linq;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.Containers
{
    public class Sequence : Container
    {
        [State] public int? CurrentIndex { get; set; }
    }

    public class SequenceDriver : NodeDriver<Sequence>, INotifyNodeExecuted
    {
        protected override INodeExecutionResult Execute(Sequence node, NodeExecutionContext context)
        {
            var currentIndex = node.CurrentIndex ?? 0;
            var childNodes = node.Nodes.ToList();

            if (currentIndex > childNodes.Count - 1)
            {
                // Reset index
                node.CurrentIndex = null;
                return Done();
            }

            var nextChildNode = childNodes[currentIndex];
            node.CurrentIndex = currentIndex + 1;
            return ScheduleNode(nextChildNode);
        }

        public ValueTask HandleNodeExecuted(NodeExecutionContext childContext, INode owner)
        {
            // Re-schedule sequence.
            childContext.ScheduleNode(owner);
            return new ValueTask();
        }
    }
}