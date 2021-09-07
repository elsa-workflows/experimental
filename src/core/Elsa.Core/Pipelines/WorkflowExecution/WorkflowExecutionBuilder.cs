using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Pipelines.WorkflowExecution
{
    public class WorkflowExecutionBuilder : IWorkflowExecutionBuilder
    {
        private const string ServicesKey = "workflow-execution.Services";
        private readonly IList<Func<WorkflowMiddlewareDelegate, WorkflowMiddlewareDelegate>> _components = new List<Func<WorkflowMiddlewareDelegate, WorkflowMiddlewareDelegate>>();

        public WorkflowExecutionBuilder(IServiceProvider serviceProvider)
        {
            ApplicationServices = serviceProvider;
        }

        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();

        public IServiceProvider ApplicationServices
        {
            get => GetProperty<IServiceProvider>(ServicesKey)!;
            set => SetProperty(ServicesKey, value);
        }

        public IWorkflowExecutionBuilder Use(Func<WorkflowMiddlewareDelegate, WorkflowMiddlewareDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }
        
        public WorkflowMiddlewareDelegate Build()
        {
            WorkflowMiddlewareDelegate pipeline = _ => new ValueTask();

            foreach (var component in _components.Reverse()) 
                pipeline = component(pipeline);

            return pipeline;
        }

        private T? GetProperty<T>(string key) => Properties.TryGetValue(key, out var value) ? (T?)value : default(T);
        private void SetProperty<T>(string key, T value) => Properties[key] = value;
    }
}