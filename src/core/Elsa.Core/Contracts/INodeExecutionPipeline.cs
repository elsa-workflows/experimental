using System;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface INodeExecutionPipeline
    {
        ActivityMiddlewareDelegate Setup(Action<INodeExecutionBuilder> setup);
        Task ExecuteAsync(ActivityExecutionContext context);
    }
}