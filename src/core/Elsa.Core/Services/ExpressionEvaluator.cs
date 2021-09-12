using System;
using System.Threading.Tasks;
using Elsa.Contracts;
using Elsa.Models;
using Microsoft.Extensions.Logging;

namespace Elsa.Services
{
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        private readonly IExpressionHandlerRegistry _registry;

        public ExpressionEvaluator(IExpressionHandlerRegistry registry)
        {
            _registry = registry;
        }

        public async ValueTask<T?> EvaluateAsync<T>(IExpression input, ExpressionExecutionContext context)
        {
            var handler = _registry.GetHandler<T>(input);

            if (handler != null) 
                return await handler.EvaluateAsync<T>(input, context);
            
            var expressionType = input.GetType().GetGenericTypeDefinition();
            throw new InvalidOperationException($"Could not find handler for expression type {expressionType}");
        }
    }
}