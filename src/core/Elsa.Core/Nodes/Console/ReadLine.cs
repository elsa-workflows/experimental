using Elsa.Attributes;
using Elsa.Models;
using Elsa.Services;

namespace Elsa.Nodes.Console
{
    public class ReadLine : CodeActivity
    {
        [Output] public string? Output { get; set; }
    }

    public class ReadLineDriver : ActivityDriver<ReadLine>
    {
        protected override void Execute(ReadLine activity, ActivityExecutionContext context)
        {
            activity.Output = System.Console.ReadLine();
        }
    }
}