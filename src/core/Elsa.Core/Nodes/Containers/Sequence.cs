using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.Containers
{
    public class Sequence : Container
    {
    }
    
    public class SequenceDriver : NodeDriver<Sequence>
    {
        protected override ValueTask<INodeExecutionResult> ExecuteAsync(Sequence node, NodeExecutionContext context)
        {
            var nodes = node.Nodes.Reverse();
            var result = ScheduleNodes(nodes);
            return new ValueTask<INodeExecutionResult>(result);
        }
    }
}