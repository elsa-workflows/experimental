using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Contracts;
using Elsa.Models;
using Elsa.Services;
using static Elsa.Results.NodeExecutionResults;

namespace Elsa.Nodes.Console
{
    public class ReadLine : INode
    {
        [Output] public string? Output { get; set; }
    }

    public class ReadLineDriver : NodeDriver<ReadLine>
    {
        protected override ValueTask<INodeExecutionResult> ExecuteAsync(ReadLine node, NodeExecutionContext context)
        {
            node.Output = System.Console.ReadLine();
            return new ValueTask<INodeExecutionResult>(Done());
        }
    }
}