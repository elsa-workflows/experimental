using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Contracts;

namespace Elsa.Pipelines.ActivityExecution
{
    public class NodeExecutionBuilder : INodeExecutionBuilder
    {
        private const string ServicesKey = "node-execution.Services";
        private readonly IList<Func<ActivityMiddlewareDelegate, ActivityMiddlewareDelegate>> _components = new List<Func<ActivityMiddlewareDelegate, ActivityMiddlewareDelegate>>();

        public NodeExecutionBuilder(IServiceProvider serviceProvider)
        {
            ApplicationServices = serviceProvider;
        }

        public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();

        public IServiceProvider ApplicationServices
        {
            get => GetProperty<IServiceProvider>(ServicesKey)!;
            set => SetProperty(ServicesKey, value);
        }

        public INodeExecutionBuilder Use(Func<ActivityMiddlewareDelegate, ActivityMiddlewareDelegate> middleware)
        {
            _components.Add(middleware);
            return this;
        }
        
        public ActivityMiddlewareDelegate Build()
        {
            ActivityMiddlewareDelegate pipeline = _ => new ValueTask();

            foreach (var component in _components.Reverse()) 
                pipeline = component(pipeline);

            return pipeline;
        }

        private T? GetProperty<T>(string key) => Properties.TryGetValue(key, out var value) ? (T?)value : default(T);
        private void SetProperty<T>(string key, T value) => Properties[key] = value;
    }
}