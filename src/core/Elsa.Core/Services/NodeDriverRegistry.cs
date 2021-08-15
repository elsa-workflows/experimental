using System;
using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elsa.Services
{
    public class NodeDriverRegistry : INodeDriverRegistry
    {
        private readonly IServiceProvider _serviceProvider;

        public NodeDriverRegistry(IOptions<WorkflowEngineOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Dictionary = new Dictionary<Type, Type>(options.Value.NodeDrivers);
        }
        
        private IDictionary<Type, Type> Dictionary { get; }
        public void Register(Type nodeType, Type driverType) => Dictionary.Add(nodeType, driverType);
        public INodeDriver? GetDriver(INode node) => GetDriver(node.GetType());
        
        public INodeDriver? GetDriver(Type nodeType)
        {
            if (!Dictionary.TryGetValue(nodeType, out var driverType))
                return null;

            return (INodeDriver)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, driverType);
        }
    }
}