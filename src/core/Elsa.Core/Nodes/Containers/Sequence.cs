using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Nodes.Containers
{
    public class Sequence : Container
    {
    }

    public class SequenceDriver : NodeDriver<Sequence>
    {
        protected override void Execute(Sequence node, NodeExecutionContext context)
        {
            var childNodes = node.Nodes.ToList();
            var firstNode = childNodes.FirstOrDefault();

            if (firstNode != null)
                context.ScheduleNode(firstNode, OnChildComplete);
        }

        private ValueTask OnChildComplete(NodeExecutionContext childContext, INode owner)
        {
            var sequence = (Sequence)owner;
            var childNodes = sequence.Nodes.ToList();
            var completedNode = childContext.Node;
            var nextIndex = childNodes.IndexOf(completedNode) + 1;

            if (nextIndex < childNodes.Count)
            {
                var nextNode = childNodes.ElementAt(nextIndex);
                childContext.WorkflowExecutionContext.Schedule(nextNode, owner, OnChildComplete);
            }

            return new ValueTask();
        }
    }
}