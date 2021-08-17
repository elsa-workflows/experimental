using System;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeExecutionPipeline
    {
        ExecuteNodeMiddlewareDelegate Setup(Action<INodeExecutionBuilder> setup);
        Task ExecuteAsync(NodeExecutionContext context);
    }
}