using System;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts
{
    public interface IActivityExecutionPipeline
    {
        ActivityMiddlewareDelegate Setup(Action<IActivityExecutionBuilder> setup);
        ActivityMiddlewareDelegate Pipeline { get; }
        Task ExecuteAsync(ActivityExecutionContext context);
    }
}