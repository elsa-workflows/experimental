using Elsa.Runtime.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Runtime.Services
{
    public class WorkflowEngineBuilder
    {
        public IServiceCollection Services { get; } = new ServiceCollection();
        
        public IWorkflowEngine BuildWorkflowEngine()
        {
            var serviceProvider = Services.BuildServiceProvider();
            return new WorkflowEngine(serviceProvider);
        }
    }
}