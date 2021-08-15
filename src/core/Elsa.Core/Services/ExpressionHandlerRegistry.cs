using System;
using System.Collections.Generic;
using Elsa.Contracts;
using Elsa.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elsa.Services
{
    public class ExpressionHandlerRegistry : IExpressionHandlerRegistry
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpressionHandlerRegistry(IOptions<WorkflowEngineOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Dictionary = new Dictionary<Type, Type>(options.Value.ExpressionHandlers);
        }

        private IDictionary<Type, Type> Dictionary { get; }

        public void Register(Type expression, Type handler) => Dictionary.Add(expression, handler);

        public IExpressionHandler? GetHandler<T>(IExpression<T> expression)
        {
            var expressionType = expression.GetType().GetGenericTypeDefinition();

            if (!Dictionary.TryGetValue(expressionType, out var handlerType))
                return null;

            return (IExpressionHandler)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, handlerType);
        }
    }
}