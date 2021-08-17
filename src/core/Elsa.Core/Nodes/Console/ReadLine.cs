using Elsa.Attributes;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Nodes.Console
{
    public class ReadLine : Node
    {
        [Output] public string? Output { get; set; }
    }

    public class ReadLineDriver : NodeDriver<ReadLine>
    {
        protected override void Execute(ReadLine node, NodeExecutionContext context)
        {
            node.Output = System.Console.ReadLine();
        }
    }
}