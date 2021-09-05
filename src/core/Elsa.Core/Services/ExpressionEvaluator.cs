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
        private readonly ILogger<ExpressionEvaluator> _logger;

        public ExpressionEvaluator(IExpressionHandlerRegistry registry, ILogger<ExpressionEvaluator> logger)
        {
            _registry = registry;
            _logger = logger;
        }

        public async ValueTask<T> EvaluateAsync<T>(IExpression<T> expression, ExpressionExecutionContext context)
        {
            var handler = _registry.GetHandler(expression);

            if (handler != null) 
                return await handler.EvaluateAsync(expression, context);
            
            var expressionType = expression.GetType().GetGenericTypeDefinition();
            throw new InvalidOperationException($"Could not find handler for expression type {expressionType}");
        }
    }
}