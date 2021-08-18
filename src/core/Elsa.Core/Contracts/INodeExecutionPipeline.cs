using System;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeExecutionPipeline
    {
        NodeMiddlewareDelegate Setup(Action<INodeExecutionBuilder> setup);
        Task ExecuteAsync(NodeExecutionContext context);
    }
}