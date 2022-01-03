using System;
using System.Threading.Tasks;
using Elsa.Models;

namespace Elsa.Contracts;

public interface IExpressionHandler
{
    ValueTask<object?> EvaluateAsync(IExpression expression, Type returnType, ExpressionExecutionContext context);
}